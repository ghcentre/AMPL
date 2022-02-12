using System;

namespace Ampl.Core
{
    /// <summary>
    /// Contains extension methods for the <see cref="DisposableBag{T}"/>.
    /// </summary>
    public static class DisposableBag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableBag{T}"/> with no assigned value.
        /// </summary>
        /// <returns><see cref="DisposableBag{T}"/> with value assigned to <see langword="null"/>.</returns>
        public static DisposableBag<object> Create()
        {
            return new DisposableBag<object>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableBag{T}"/> with the value specified in the parameter.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to store in the bag.</param>
        /// <returns><see cref="DisposableBag{T}"/>.</returns>
        public static DisposableBag<T> Create<T>(T value)
        {
            return new DisposableBag<T>(value);
        }

        /// <summary>
        /// Adds the disposable object to the collection of objects which are disposed when the bag is disposed.
        /// </summary>
        /// <typeparam name="T">The type of the value stored in the bag.</typeparam>
        /// <param name="bag">The bag.</param>
        /// <param name="disposable">The disposable object.</param>
        /// <returns>The bag.</returns>
        /// <exception cref="ArgumentNullException">The bag is <see langword="null"/>,
        /// or the disposable object is <see langword="null"/>.</exception>
        public static DisposableBag<T> With<T>(this DisposableBag<T> bag, IDisposable disposable)
        {
            Guard.Against.Null(bag, nameof(bag));
            Guard.Against.Null(disposable, nameof(disposable));

            bag.Add(disposable.Dispose);

            return bag;
        }

        /// <summary>
        /// Adds the action to the collection of actions which are executed when the bag is disposed.
        /// </summary>
        /// <typeparam name="T">The type of the value stored in the bag.</typeparam>
        /// <param name="bag">The bag.</param>
        /// <param name="action">The action</param>
        /// <returns>The bag.</returns>
        /// <exception cref="ArgumentNullException">The bag is <see langword="null"/>,
        /// or the action is <see langword="null"/>.</exception>
        public static DisposableBag<T> With<T>(this DisposableBag<T> bag, Action action)
        {
            Guard.Against.Null(bag, nameof(bag));
            Guard.Against.Null(action, nameof(action));

            bag.Add(action);

            return bag;
        }

        /// <summary>
        /// Adds the collection of disposables to the bag. Disposables are disposed when the bag is disposed.
        /// </summary>
        /// <typeparam name="T">The type of the value stored in the bag.</typeparam>
        /// <param name="bag">The bag.</param>
        /// <param name="disposables">Sequence of disposables.</param>
        /// <returns>The bag.</returns>
        /// <exception cref="ArgumentNullException">The bag is <see langword="null"/>,
        /// or the sequence of disposables is <see langword="null"/>.</exception>
        public static DisposableBag<T> With<T>(this DisposableBag<T> bag, params IDisposable[] disposables)
        {
            Guard.Against.Null(bag, nameof(bag));
            Guard.Against.Null(disposables, nameof(disposables));

            foreach (var disposable in disposables)
            {
                bag.With(disposable); // will check for null
            }

            return bag;
        }

        /// <summary>
        /// Adds the collection of actions to the bag. Actions are executed when the bag is disposed.
        /// </summary>
        /// <typeparam name="T">The type of the value stored in the bag.</typeparam>
        /// <param name="bag">The bag.</param>
        /// <param name="actions">The sequence of actions.</param>
        /// <returns>The bag.</returns>
        /// <exception cref="ArgumentNullException">The bag is <see langword="null"/>,
        /// or the sequence of actions is <see langword="null"/>.</exception>
        public static DisposableBag<T> With<T>(this DisposableBag<T> bag, params Action[] actions)
        {
            Guard.Against.Null(bag, nameof(bag));
            Guard.Against.Null(actions, nameof(actions));

            foreach (var action in actions)
            {
                bag.With(action); // will check for null
            }

            return bag;
        }
    }
}
