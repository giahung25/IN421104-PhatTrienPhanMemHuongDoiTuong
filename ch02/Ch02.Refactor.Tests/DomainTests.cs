using EduHub.Ch02;
using EduHub.Ch02.Domain;

namespace Ch02.Refactor.Tests;

// Mẫu AAA: Arrange (chuẩn bị) - Act (thực hiện) - Assert (kiểm tra).
// Mỗi test chỉ kiểm tra MỘT hành vi. Đặt tên: TênMethod_TìnhHuống_KếtQuảMongĐợi.
public class DomainTests
{
    // Test mẫu ban đầu: kiểm tra ngoại lệ tên rỗng qua ClubManager
    [Fact]
    public void CreateClub_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var manager = new ClubManager();

        // Act
        var act = () => manager.CreateClub("", "mô tả", "p@gmail.com");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Club_Create_WithValidName_ReturnsClub()
    {
        // Arrange
        const string name = "Tech Club";
        const string description = "Câu lạc bộ công nghệ";

        // Act
        var club = Club.Create(name, description);

        // Assert
        Assert.NotNull(club);
        Assert.Equal(name, club.Name);
        Assert.Equal(description, club.Description);
        Assert.NotEqual(Guid.Empty, club.Id);
    }

    [Fact]
    public void Club_Create_WithInvalidName_ThrowsArgumentException()
    {
        // Arrange
        var longName = new string('a', Club.MaxNameLength + 1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Club.Create("", "mô tả"));
        Assert.Throws<ArgumentException>(() => Club.Create("   ", "mô tả"));
        Assert.Throws<ArgumentException>(() => Club.Create(longName, "mô tả"));
    }

    [Fact]
    public void Club_HasRoom_AtMaxMembers_ReturnsFalse()
    {
        // Arrange
        var club = Club.Create("Tech Club", "mô tả");

        // Act & Assert
        Assert.True(club.HasRoom(0));
        Assert.True(club.HasRoom(49));
        Assert.False(club.HasRoom(50));
        Assert.False(club.HasRoom(51));
    }

    [Fact]
    public void Event_Register_NewMember_ReturnsTrue()
    {
        // Arrange
        var ev = new Event { Id = Guid.NewGuid(), ClubId = Guid.NewGuid(), Title = "Workshop SOLID", Date = DateTime.Now };
        var memberId = Guid.NewGuid();

        // Act
        var result = ev.Register(memberId);

        // Assert
        Assert.True(result);
        Assert.Contains(memberId, ev.RegisteredMemberIds);
    }

    [Fact]
    public void Event_Register_SameMemberTwice_ReturnsFalse()
    {
        // Arrange
        var ev = new Event { Id = Guid.NewGuid(), ClubId = Guid.NewGuid(), Title = "Workshop SOLID", Date = DateTime.Now };
        var memberId = Guid.NewGuid();
        ev.Register(memberId);

        // Act
        var result = ev.Register(memberId);

        // Assert
        Assert.False(result);
        Assert.Single(ev.RegisteredMemberIds);
    }

    [Fact]
    public void Event_CheckIn_UnregisteredMember_ThrowsInvalidOperationException()
    {
        // Arrange
        var ev = new Event { Id = Guid.NewGuid(), ClubId = Guid.NewGuid(), Title = "Workshop SOLID", Date = DateTime.Now };
        var unregisteredMemberId = Guid.NewGuid();

        // Act
        var act = () => ev.CheckIn(unregisteredMemberId);

        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }
}
