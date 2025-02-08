using System.ClientModel;
using OpenAI;
using OpenAI.Chat;
using Pilot.BlazorClient.Interface;
using Pilot.BlazorClient.ViewModels;

namespace Pilot.BlazorClient.Service;

public class AiService : IAiService
{
    private const string Model = "qwen2.5";
    private const string Uri = "http://localhost:11434/v1";
    private readonly ChatClient _client;
    private readonly ICollection<ChatMessage> _history = [];
    public AiService()
    {
        _history.Add(new SystemChatMessage("Проект представляет собой микросервисную платформу Pilot, предназначенную для управления проектами, задачами и коммуникацией между участниками команд. Она ориентирована на повышение эффективности работы организаций, прозрачности процессов управления и удобства взаимодействия сотрудников.  \n\n### Основные возможности платформы Pilot:  \n\n1. Управление проектами  \n   - Возможность создавать проекты, задавать их параметры (цели, сроки, описание).  \n   - Просмотр активных и завершённых проектов.  \n   - Организация проектов в зависимости от команд и направлений.  \n\n2. Управление задачами  \n   - Создание задач с указанием дедлайнов, исполнителей и статусов.  \n   - Отслеживание прогресса выполнения задач.  \n   - Комментирование задач для совместной работы.  \n\n3. Командная работа  \n   - Управление командами: добавление участников, назначение ролей.  \n   - Возможность совместного обсуждения задач через встроенный чат.  \n\n4. Коммуникация и уведомления  \n   - Реализован чат для обсуждения задач, проектов и оперативного взаимодействия.  \n   - Уведомления об изменениях в проектах и задачах для всех участников команды.  \n\n5. Прозрачность управления  \n   - Инструменты для отслеживания изменений, истории правок и текущих статусов задач.  \n   - Аналитика: отчёты по выполнению задач, статистика работы команд и участников.  \n\n---\n\n### Как воспользоваться возможностями платформы?  \n\n1. Начальная работа  \n   - Перейдите на главную страницу платформы (например, `/dashboard`). Здесь отображается список активных проектов и задач.  \n\n2. Создание проектов  \n   - В разделе \"Проекты\" на странице `/projects` можно создать новый проект, указав его название, сроки и участников.  \n\n3. Управление задачами  \n   - Для добавления новой задачи перейдите в раздел \"Задачи\" на странице `/tasks`. Здесь можно указать параметры задачи и назначить исполнителей.  \n   - Для редактирования или отслеживания прогресса задач выберите соответствующую задачу из списка.  \n\n4. Командная работа  \n   - В разделе \"Команды\" на странице `/teams` можно добавить участников в проект, назначить роли и распределить задачи.  \n   - Для общения с командой используйте встроенный чат, доступный на странице каждого проекта.  \n\n5. Аналитика и отчёты  \n   - В разделе \"Отчёты\" на странице `/reports` можно посмотреть статистику работы команды, прогресс выполнения задач и общую эффективность работы над проектами.  \n\n6. Уведомления  \n   - На странице \"Уведомления\" `/notifications` отображаются все изменения, включая обновления задач, сообщения из чатов и статусы выполнения проектов.  \n\nЭти возможности позволяют эффективно организовать рабочие процессы, улучшить коммуникацию между участниками и обеспечить прозрачность управления на всех этапах выполнения проекта.  "));
        _history.Add(new SystemChatMessage("Пиши свои ответы без абзацев и отступов в виде одного сплошного текста"));
        var clientOptions = new OpenAIClientOptions
        {
            Endpoint = new Uri(Uri)
        };

        _client = new ChatClient(Model, new ApiKeyCredential("Should be empty"), clientOptions);
    }

    public async Task SendMessageAsync(string prompt, MessageViewModel message,
        Action continuedMessage)
    {
        _history.Add(new UserChatMessage(prompt));
        AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates =
            _client.CompleteChatStreamingAsync(_history);
        
        await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
        {
            if (completionUpdate.ContentUpdate.Count > 0)
            {
                if (message.Text != null && message.Text.Contains("Думаю")) message.Text = string.Empty;

                message.Text += completionUpdate.ContentUpdate[0].Text.Replace("**", "").Replace("\n", "");
                continuedMessage.Invoke();
                Console.Write(completionUpdate.ContentUpdate[0].Text);
            }
        }
        _history.Add(new AssistantChatMessage(message.Text));
    }
}