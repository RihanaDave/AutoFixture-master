﻿using System;
using AutoFixture.Kernel;

namespace AutoFixture.AutoNSubstitute
{
    /// <summary>Provides pre- and post-condition checks for requests for substituted instances.</summary>
    /// <seealso cref="Create(object, ISpecimenContext)"/>
    public class NSubstituteBuilder : ISpecimenBuilder
    {
        /// <summary>Initializes a new instance of the <see cref="NSubstituteBuilder"/> class with an
        ///     <see cref="ISpecimenBuilder"/> to decorate.</summary>
        /// <param name="builder">The builder which must build mock instances.</param>
        /// <remarks>
        ///     <paramref name="builder"/> is subsequently available through the <see cref="Builder"/> property.
        /// </remarks>
        /// <seealso cref="Builder"/>
        public NSubstituteBuilder(ISpecimenBuilder builder)
            : this(builder, new AbstractTypeSpecification())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NSubstituteBuilder"/> class with an
        ///     <see cref="ISpecimenBuilder"/> to decorate.</summary>
        /// <param name="builder">The builder which must build mock instances.</param>
        /// <param name="substitutionSpecification">A specification that determines whether a substitute should be created for a given request or not.</param>
        /// <remarks>
        ///     <para><paramref name="builder"/> is subsequently available through the <see cref="Builder"/> property.</para>
        ///     <para><paramref name="substitutionSpecification"/> is subsequently available through the <see cref="SubstitutionSpecification"/> property.</para>
        /// </remarks>
        /// <seealso cref="Builder"/>
        /// <seealso cref="SubstitutionSpecification"/>
        public NSubstituteBuilder(ISpecimenBuilder builder, IRequestSpecification substitutionSpecification)
        {
            this.Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.SubstitutionSpecification = substitutionSpecification ?? throw new ArgumentNullException(nameof(substitutionSpecification));
        }

        /// <summary>Gets the decorated builder supplied through the constructor.</summary>
        /// <seealso cref="NSubstituteBuilder(ISpecimenBuilder)"/>
        public ISpecimenBuilder Builder { get; }

        /// <summary>Gets a specification that determines whether a substitute should be created for a given request.</summary>
        /// <remarks>
        ///     <para>
        ///         By default it only returns <see langword="true"/> for interfaces and abstract classes, but a different specification can be supplied by using the
        ///         <see cref="NSubstituteBuilder(ISpecimenBuilder, IRequestSpecification)"/> overloaded constructor that takes a specification as input. In that case, this
        ///         property returns the specification supplied to the constructor.
        ///     </para>
        /// </remarks>
        /// <seealso cref="NSubstituteBuilder(ISpecimenBuilder, IRequestSpecification)"/>
        public IRequestSpecification SubstitutionSpecification { get; }

        /// <summary>Creates a new specimen based on a request.</summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.</returns>
        /// <remarks>
        ///     If the request is not a <see cref="Type"/> or the request does not represent an interface or an abstract class, this method returns a new
        ///     <see cref="NoSpecimen"/>; otherwise, it returns a substitute for the requested type.
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (!this.SubstitutionSpecification.IsSatisfiedBy(request))
                return new NoSpecimen();

            var substitute = this.Builder.Create(request, context);
            if (substitute == null)
                return new NoSpecimen();

            return substitute;
        }
    }
}
