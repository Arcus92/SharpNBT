using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using SuppressMessageAttribute =  System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;

namespace SharpNBT
{
    /// <summary>
    /// Base class for tags that contain a collection of other <see cref="Tag"/> objects and can be enumerated.
    /// </summary>
    [PublicAPI][DataContract]
    public abstract class TagContainer : EnumerableTag<Tag>
    {
        protected bool NamedChildren;
        protected TagType? RequiredType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagContainer"/>.
        /// </summary>
        /// <param name="type">A constant describing the NBT type for this tag.</param>
        /// <param name="name">The name of the tag, or <see langword="null"/> if tag has no name.</param>
        protected TagContainer(TagType type, [CanBeNull] string name) : base(type, name)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagContainer"/> with the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="type">A constant describing the NBT type for this tag.</param>
        /// <param name="name">The name of the tag, or <see langword="null"/> if tag has no name.</param>
        /// <param name="values">A collection of values to include in this tag.</param>
        protected TagContainer(TagType type, [CanBeNull] string name, [NotNull][ItemNotNull] IEnumerable<Tag> values) : base(type, name, values)
        {
        }

        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
        /// <footer><a href="https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.ICollection-1.Add?view=netcore-5.0">`ICollection.Add` on docs.microsoft.com</a></footer>
        [SuppressMessage("ReSharper", "AnnotationConflictInHierarchy")]
        public sealed override void Add(Tag item)
        {
            base.Add(AssertConventions(item));
            item.Parent = this;
        }
        
        /// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        /// <footer><a href="https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IList-1.Insert?view=netcore-5.0">`IList.Insert` on docs.microsoft.com</a></footer>
        [SuppressMessage("ReSharper", "AnnotationConflictInHierarchy")]
        public sealed override void Insert(int index, Tag item)
        {
            base.Insert(index, AssertConventions(item));
            item.Parent = this;
        }
        
        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        /// <returns>The element at the specified index.</returns>
        /// <footer><a href="https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IList-1.Item?view=netcore-5.0">`IList.Item` on docs.microsoft.com</a></footer>
        public sealed override Tag this[int index]
        {
            get => base[index];
            set
            {
                base[index] = AssertConventions(value);
                value.Parent = this;
            }
        }
        
        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
        /// <footer><a href="https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.ICollection-1.Clear?view=netcore-5.0">`ICollection.Clear` on docs.microsoft.com</a></footer>
        public sealed override void Clear()
        {
            foreach (var item in this)
                item.Parent = null;
            base.Clear();
        }
        
        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</exception>
        /// <returns>
        /// <see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        /// <footer><a href="https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.ICollection-1.Remove?view=netcore-5.0">`ICollection.Remove` on docs.microsoft.com</a></footer>
        public sealed override bool Remove(Tag item)
        {
            if (item is null || !base.Remove(item))
                return false;
            
            item.Parent = null;
            return true;
        }
        
        /// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.</summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1" /> is read-only.</exception>
        /// <footer><a href="https://docs.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IList-1.RemoveAt?view=netcore-5.0">`IList.RemoveAt` on docs.microsoft.com</a></footer>
        public sealed override void RemoveAt(int index)
        {
            this[index].Parent = null;
            base.RemoveAt(index);
        }

        /// <summary>
        /// Performs routine checks to ensure that the given <paramref name="tag"/> complies with the NBT standard for this collection type.
        /// </summary>
        /// <param name="tag">A <see cref="Tag"/> instance to validate.</param>
        /// <returns>Returns the <paramref name="tag"/> instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArrayTypeMismatchException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Tag AssertConventions([CanBeNull] Tag tag)
        {
            if (tag is null)
                throw new ArgumentNullException(nameof(tag), "Child tag in collection cannot be null");

            if (NamedChildren && tag.Name is null)
                throw new FormatException("Children of this collection type must be named.");
            if (!NamedChildren && tag.Name != null)
                throw new FormatException("Children of this collection type cannot be named.");

            if (RequiredType.HasValue && RequiredType.Value != tag.Type)
                throw new ArrayTypeMismatchException("Incorrect tag type added to this collection.");

            return tag;
        }
    }
}