using Microsoft.Extensions.Configuration;
using Moq;
using RoeiVerenigingLibary;


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
            var member = new Member(1, "simon", "wil roeien", "simon@windesheim.nl", new List<string>());
            var memberRepository = new Mock<IMemberRepository>();
            memberRepository.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(member);
            var memberService = new MemberService(memberRepository.Object);
            var result =  memberService.Login("simon@windesheim.nl", "Test1234");

            Assert.AreEqual(result, member);
        }
    }
}
