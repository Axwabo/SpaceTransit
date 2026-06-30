using System.Threading.Tasks;
using UnityEngine;

public static class AwaitableExtensions
{

    public static async Awaitable WhenAll(Awaitable first, Awaitable second) => await Task.WhenAll(first.AsTask(), second.AsTask());

    private static async Task AsTask(this Awaitable awaitable) => await awaitable;

}
