public class EventProduct
{
    public event EventHandler<string> ProcessCompleted; 
 
    public void Trigger(string  pesan)
    {
        OnProcessCompleted(pesan);
    }
 
    protected virtual void OnProcessCompleted(string  pesan)
    {
        ProcessCompleted?.Invoke(this, pesan);
    }
}