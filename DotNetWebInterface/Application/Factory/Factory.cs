using DotNetWebInterface.Server;
using DotNetWebInterface.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetWebInterface
{
    /// <summary>
    /// Factory class to create instances of specified types
    /// </summary>
    internal static class Factory
    {
        /// <summary>
        /// Creates an instance of the specified type with the given arguments
        /// </summary>
        /// <typeparam name="T">The type of the instance to create</typeparam>
        /// <param name="type">The type of the instance to create</param>
        /// <param name="args">The arguments to pass to the constructor of the instance</param>
        /// <returns>An instance of the specified type</returns>
        internal static T Create<T>(Type type, params object[] args) where T : class
        {
            var serviceProvider = ServiceContainer.Resolve<IServiceProvider>();
            var instance = (T)ActivatorUtilities.CreateInstance(serviceProvider, type, args);

            return instance;
        }

        /// <summary>
        /// Resolves and creates an instance of the specified type, executing the given action within the same synchronization context.
        /// </summary>
        /// <typeparam name="T">The type of the object to create.</typeparam>
        /// <param name="type">The type of the object to instantiate.</param>
        /// <param name="action">The action to execute using the created instance.</param>
        /// <param name="args">Optional parameters for the object constructor.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal static async Task CreateAsync<T>(Type type, Func<T, Task> action, params object[] args) where T : class
        {
            await ServiceContainer.ResolveScopedServiceAsync<IServiceProvider>(async serviceProvider =>
            {
                try
                {
                    var instance = (T)ActivatorUtilities.CreateInstance(serviceProvider, type, args);
                    if (action != null)
                    {
                        await action(instance).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during instance creation: {ex.Message}");
                    throw;
                }
            });
        }
    }
}
