using Quartz;

public class Crawl : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        // Logique de votre tâche
        Console.WriteLine("Tâche exécutée.");
        return Task.CompletedTask;
    }
}