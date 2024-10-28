using Bogus;
using Pilot.Contracts.Data;
using Pilot.Contracts.Data.Enums;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Data;

public class Seed : ISeed
{
    private readonly DataContext _context;

    public Seed(DataContext context)
    {
        _context = context;
    }

    public async Task Seeding()
    {
        if (_context.Chats.Any()) return;

        // Assuming you have user IDs from 1 to 30 in your database
        var userIds = Enumerable.Range(1, 30).ToList();

        // Grouping user IDs by companies (6 users per company)
        var userGroups = userIds
            .Select((id, index) => new { id, group = index / 6 })
            .GroupBy(x => x.group)
            .Select(g => g.Select(x => x.id).ToList())
            .ToList();

        // Generating chats and messages for each group
        foreach (var group in userGroups)
        {
            // Create a chat with the first user in the group as the creator
            var chat = GenerateChat(group[0]);

            // Adding each user in the group as a chat member
            chat.ChatMembers = group.Select(userId => new ChatMember { UserId = userId, Chat = chat }).ToList();

            // Generate messages for the chat
            chat.Messages = GenerateMessages(chat, group);

            _context.Chats.Add(chat);
        }

        // Save changes to the database
        await _context.SaveChangesAsync();
    }

    #region Faker Generators

    private Chat GenerateChat(int createdByUserId)
    {
        var chatFaker = new Faker<Chat>()
            .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
            .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
            .RuleFor(c => c.CreatedBy, _ => createdByUserId);

        return chatFaker.Generate();
    }

    private List<Message> GenerateMessages(Chat chat, List<int> group)
    {
        var messageFaker = new Faker<Message>()
            .RuleFor(m => m.Text, f => f.Lorem.Sentence())
            .RuleFor(m => m.Chat, _ => chat)
            .RuleFor(m => m.UserId, f => f.PickRandom(group));

        return messageFaker.Generate(10); // Generate 10 messages per chat
    }

    private InfoMessage GenerateInfoMessage(int userId)
    {
        var infoMessageFaker = new Faker<InfoMessage>()
            .RuleFor(im => im.Title, f => f.Lorem.Sentence(2))
            .RuleFor(im => im.Description, f => f.Lorem.Paragraph())
            .RuleFor(im => im.UserId, _ => userId)
            .RuleFor(im => im.MessagePriority, f => f.PickRandom<MessageInfo>());

        return infoMessageFaker.Generate();
    }

    #endregion
}
