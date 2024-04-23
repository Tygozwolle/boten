using Microsoft.Extensions.Configuration;
using Moq;
using RoeiVerenigingLibary;
using RoeiVerenigingLibary.Exceptions;


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
            var member = new Member(1, "simon", "wil roeien", "simon@windesheim.nl", new List<string>());
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(member);
            var memberService = new MemberService(memberRepository.Object);
            //Act
            var result = memberService.Login("simon@windesheim.nl", "Test1234");
            //Assert
            Assert.AreEqual(result, member);
        }

        [Test]
        public void InlogFailed()
        {
            //Arrange
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Get("simon@windesheim.nl", "Test1234")).Returns((Member)null);
            var memberService = new MemberService(memberRepository.Object);
            //Act and Assert
            Assert.Throws<IncorrectEmailOrPasswordException>(() => memberService.Login("simon@windesheim.nl", "Test1234"));
        }

        [Test]
        public void CreateMemberSuccesfull()
        {
            //Arrange
            var member = new Member(1, "tygo", "olst van", "tygo@windesheim.nl", new List<string>());
            var admin = new Member(1, "simon", "van den Berg", "simon@windesheim.nl", new List<string>());
            admin.Roles.Add("beheerder");
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(member);
            var memberService = new MemberService(memberRepository.Object);
            //Act
            var result = memberService.Create(admin, "tygo", "olst van", "tygo@windesheim.nl", "Test1234");
            //Assert
            Assert.AreEqual(result, member);
        }

        [Test]
        public void CreateMemberAlreadyExist()
        {
            //Arrange
            var admin = new Member(1, "simon", "van den Berg", "simon@windesheim.nl", new List<string>());
            admin.Roles.Add("beheerder");
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((Member)null);
            var memberService = new MemberService(memberRepository.Object);
            //Act and Assert
            Assert.Throws<MemberAlreadyExistsException> (() => memberService.Create(admin ,"tygo", "van olst", "tygo@windesheim.nl", "Test1234"));
        }
        [Test]
        public void CreateMemberNoAdmin()
        {
            //Arrange
            var admin = new Member(1, "simon", "van den Berg", "simon@windesheim.nl", new List<string>());
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((Member)null);
            var memberService = new MemberService(memberRepository.Object);
            //Act and Assert
            Assert.Throws<IncorrectRightsExeption>(() => memberService.Create(admin, "tygo", "van olst", "tygo@windesheim.nl", "Test1234"));
        }
    }
}
