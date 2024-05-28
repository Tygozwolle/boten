using Moq;
using RoeiVerenigingLibrary;
using RoeiVerenigingLibrary.Exceptions;

namespace RoeiVerenigingUnitTests
{
    public class MemberTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InlogSuccesfull()
        {
            //Arrange
            Member member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(member);
            MemberService memberService = new MemberService(memberRepository.Object);
            //Act
            Member result = memberService.Login("simon@windeheim.nl", "Test1234");
            //Assert
            Assert.That(Equals(result, member));
        }

        [Test]
        public void InlogFailed()
        {
            //Arrange
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Get("simon@windesheim.nl", "Test1234")).Returns((Member)null);
            MemberService memberService = new MemberService(memberRepository.Object);
            //Act and Assert
            Assert.Throws<IncorrectEmailOrPasswordException>(() =>
                memberService.Login("simon@windesheim.nl", "Test1234"));
        }

        [Test]
        public void CreateMemberSuccesfull()
        {
            //Arrange
            Member member = new Member(1, "tygo", "van", "olst", "tygo@windesheim.nl", new List<string>(), 1);
            Member admin = new Member(1, "simon", "van den ", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            admin.Roles.Add("beheerder");
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(member);
            MemberService memberService = new MemberService(memberRepository.Object);
            //Act
            Member result = memberService.Create(admin, "tygo", "van", "olst", "tygo@windesheim.nl", "Test1234");
            //Assert
            Assert.That(Equals(result, member));
        }

        [Test]
        public void CreateMemberAlreadyExist()
        {
            //Arrange
            Member admin = new Member(1, "simon", "van den ", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            admin.Roles.Add("beheerder");
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), 1)).Returns((Member)null);
            MemberService memberService = new MemberService(memberRepository.Object);
            //Act and Assert
            Assert.Throws<MemberAlreadyExistsException>(() =>
                memberService.Create(admin, "tygo", "van", "olst", "tygo@windesheim.nl", "Test1234"));
        }

        [Test]
        public void CreateMemberNoAdmin()
        {
            //Arrange
            Member admin = new Member(1, "simon", "van den ", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), 1)).Returns((Member)null);
            MemberService memberService = new MemberService(memberRepository.Object);
            //Act and Assert
            Assert.Throws<IncorrectRightsException>(() =>
                memberService.Create(admin, "tygo", "van", "olst", "tygo@windesheim.nl", "Test1234"));
        }

        [Test]
        public void ChangePassword_Successful()
        {
            // Arrange
            Member member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(member);
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act
            memberService.ChangePassword(member, "OldPassword", "NewPassword", "NewPassword");

            // Assert
            memberRepository.Verify(x => x.ChangePassword(member.Email, It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ChangePassword_Failed_IncorrectCurrentPassword()
        {
            // Arrange
            Member member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns((Member)null);
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act and Assert
            Assert.Throws<IncorrectPasswordException>(() =>
                memberService.ChangePassword(member, "IncorrectOldPassword", "NewPassword", "NewPassword"));
        }

        [Test]
        public void ChangePassword_Failed_PasswordsDontMatch()
        {
            // Arrange
            Member member = new Member(1, "simon", "van den", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(member);
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act and Assert
            Assert.Throws<PasswordsDontMatchException>(() =>
                memberService.ChangePassword(member, "OldPassword", "NewPassword", "DifferentNewPassword"));
        }

        [Test]
        public void GetAllRoles_Successful()
        {
            // Arrange
            var roles = new List<string> { "role1", "role2", "role3" };
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.GetAvailableRoles()).Returns(roles);
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act
            var result = memberService.GetAvailableRoles();

            // Assert
            Assert.That(result, Is.EqualTo(roles));
        }

        [Test]
        public void SetRoles_Successful()
        {
            // Arrange
            int memberId = 1;
            var roles = new List<string> { "role1", "role2", "role3" };
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.RemoveRoles(memberId)).Verifiable();
            memberRepository.Setup(x => x.AddRole(memberId, It.IsAny<string>())).Verifiable();
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act
            memberService.SetRoles(memberId, roles);

            // Assert
            memberRepository.Verify(x => x.RemoveRoles(memberId), Times.Once);
            foreach (string role in roles)
            {
                memberRepository.Verify(x => x.AddRole(memberId, role), Times.Once);
            }
        }

        [Test]
        public void UpdateMember_InvalidEmail_ThrowsException()
        {
            // Arrange
            Member admin = new Member(1, "simon", "van den ", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            admin.Roles.Add("beheerder");
            var memberRepository = new Mock<IMemberRepository>();
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act and Assert
            Assert.Throws<Exception>(() =>
                memberService.Update(admin, 1, "tygo", "van", "olst", "invalidEmail", 1));
        }

        [Test]
        public void UpdateMember_LevelGreaterThan10_ThrowsException()
        {
            // Arrange
            Member admin = new Member(1, "simon", "van den ", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            admin.Roles.Add("beheerder");
            var memberRepository = new Mock<IMemberRepository>();
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act and Assert
            Assert.Throws<Exception>(() =>
                memberService.Update(admin, 1, "tygo", "van", "olst", "tygo@windesheim.nl", 11));
        }

        [Test]
        public void UpdateMember_NotAdmin_ThrowsException()
        {
            // Arrange
            Member admin = new Member(1, "simon", "van den ", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            var memberRepository = new Mock<IMemberRepository>();
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act and Assert
            Assert.Throws<IncorrectRightsException>(() =>
                memberService.Update(admin, 1, "tygo", "van", "olst", "tygo@windesheim.nl", 1));
        }

        [Test]
        public void UpdateMember_DatabaseAccessError_ThrowsException()
        {
            // Arrange
            Member admin = new Member(1, "simon", "van den ", "Berg", "simon@windesheim.nl", new List<string>(), 1);
            admin.Roles.Add("beheerder");
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>())).Throws<Exception>();
            MemberService memberService = new MemberService(memberRepository.Object);

            // Act and Assert
            Assert.Throws<CantAccesDatabaseException>(() =>
                memberService.Update(admin, 1, "tygo", "van", "olst", "tygo@windesheim.nl", 1));
        }
    }
}