﻿using System;
using AutoFixture.Kernel;

namespace AutoFixture.DataAnnotations
{
    /// <summary>
    /// Handles the <see cref="RangedRequest"/> for the number type by forwarding it
    /// to the <see cref="RangedNumberRequest"/>.
    /// </summary>
    public class NumericRangedRequestRelay : ISpecimenBuilder
    {
        /// <inheritdoc />
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            if (request is not RangedRequest rangedRequest)
                return new NoSpecimen();

            if (!rangedRequest.MemberType.IsNumberType())
                return new NoSpecimen();

            var convertedMinimum = rangedRequest.GetConvertedMinimum(rangedRequest.MemberType);
            var convertedMaximum = rangedRequest.GetConvertedMaximum(rangedRequest.MemberType);

            var rangedNumberRequest = new RangedNumberRequest(
                rangedRequest.MemberType,
                convertedMinimum,
                convertedMaximum);

            return context.Resolve(rangedNumberRequest);
        }
    }
}