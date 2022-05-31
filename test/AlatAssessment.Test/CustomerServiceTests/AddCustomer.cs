using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.DTOs;
using AlatAssessment.Services;
using AlatAssessment.Services.Interfaces;
using AutoMapper;
using Xunit;

namespace AlatAssessment.Test.CustomerServiceTests
{
    public class AddCustomer : BaseCustomerServiceTest
    {
        private readonly AddCustomerDTO _customerDto;
        private readonly Customer _customer;

        public AddCustomer()
        {
            _customerDto = new AddCustomerDTO
            {
                PhoneNumber = "11111111111",
                Password = "12345678",
                LgaId = 1,
                StateId = 1,
                Email = "dummy@dummy.com"
            };
            _customer = new Customer()
            {
                CreatedAt = DateTime.Now,
                Email = _customerDto.Email,
                PhoneNumber = _customerDto.PhoneNumber
            };
        }

        [Fact]
        public void ShouldAddCustomer()
        {
            //Arrange 
            notificationService.Setup(x => x.SendOtp(_customerDto.PhoneNumber)).Returns(true);
            unitOfWork.Setup(x => x.CustomerRepo.Find(x => x.Email == _customerDto.Email.Trim())).Returns(new List<Customer>());
            passwordManager.Setup(x => x.GetHash(_customerDto.Password))
                .Returns(PasswordManager.CreateManager().GetHash(_customerDto.Password));

            //Act
            var actual=customerService.AddCustomer(_customerDto);

            //Assert
            Assert.Equal(actual.Result.Code,ResponseCodes.Success);
            unitOfWork.Verify(x=> x.CustomerRepo.Create(It.Is<Customer>(y=>
                y.CreatedAt !=_customer.CreatedAt &&
                y.Email==_customer.Email &&
                y.Id==new Guid() &&
                y.LgaId==_customerDto.LgaId &&
                y.IsVerified == false &&
                y.PhoneNumber == _customerDto.PhoneNumber &&
                y.ModifiedAt != _customer.ModifiedAt
            )),Times.Once);
            unitOfWork.Verify(x=> x.SaveAsync(),Times.Once);
        }


        [Fact]
        public void ShouldSendVerificationOtp_WhenCustomerIsAdded()
        { 
            //Arrange 
            notificationService.Setup(x => x.SendOtp(_customerDto.PhoneNumber)).Returns(true);
            unitOfWork.Setup(x => x.CustomerRepo.Find(x => x.Email == _customerDto.Email.Trim())).Returns(new List<Customer>());
            passwordManager.Setup(x => x.GetHash(_customerDto.Password))
                .Returns(PasswordManager.CreateManager().GetHash(_customerDto.Password));

            //Act
            var actual = customerService.AddCustomer(_customerDto);

            //Assert
            Assert.Equal(actual.Result.Code, ResponseCodes.Success);
            notificationService.Verify(x => x.SendOtp(_customer.PhoneNumber), Times.Once);
        }

        [Fact]
        public void ShouldNotAddCustomer_WhenEmailAlreadyExist()
        {
            notificationService.Setup(x => x.SendOtp(_customerDto.PhoneNumber)).Returns(true);
            unitOfWork.Setup(x => x.CustomerRepo.Find(It.IsAny<Expression<Func<Customer, bool>>>()))
                .Returns(new List<Customer>() {_customer});
            passwordManager.Setup(x => x.GetHash(_customerDto.Password))
                .Returns(PasswordManager.CreateManager().GetHash(_customerDto.Password));

            //Act
            var actual = customerService.AddCustomer(_customerDto);

            //Assert
            Assert.Equal(actual.Result.Code, ResponseCodes.Success);
            unitOfWork.Verify(x => x.CustomerRepo.Create(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public void ShouldSendNotification_WhenEmailAlreadyExistButNotVerified()
        {
            notificationService.Setup(x => x.SendOtp(_customerDto.PhoneNumber)).Returns(true);
            unitOfWork.Setup(x => x.CustomerRepo.Find(It.IsAny<Expression<Func<Customer, bool>>>()))
                .Returns(new List<Customer>() { _customer });
            passwordManager.Setup(x => x.GetHash(_customerDto.Password))
                .Returns(PasswordManager.CreateManager().GetHash(_customerDto.Password));

            //Act
            var actual = customerService.AddCustomer(_customerDto);

            //Assert
            Assert.Equal(actual.Result.Code, ResponseCodes.Success);
            unitOfWork.Verify(x => x.CustomerRepo.Create(It.IsAny<Customer>()), Times.Never);
            notificationService.Verify(x => x.SendOtp(_customer.PhoneNumber), Times.Once());
        }

        [Fact]
        public void ShouldHashPassword_WhenSaving()
        {
            //Arrange 
            notificationService.Setup(x => x.SendOtp(_customerDto.PhoneNumber)).Returns(true);
            unitOfWork.Setup(x => x.CustomerRepo.Find(x => x.Email == _customerDto.Email.Trim())).Returns(new List<Customer>());
            passwordManager.Setup(x => x.GetHash(_customerDto.Password))
                .Returns(PasswordManager.CreateManager().GetHash(_customerDto.Password));

            //Act
            var actual = customerService.AddCustomer(_customerDto);

            //Assert
            Assert.Equal(actual.Result.Code, ResponseCodes.Success);
            unitOfWork.Verify(x => x.CustomerRepo.Create(It.Is<Customer>(y =>
                y.Password == PasswordManager.CreateManager().GetHash(_customerDto.Password)
            )), Times.Once);
            unitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }


    }
}
