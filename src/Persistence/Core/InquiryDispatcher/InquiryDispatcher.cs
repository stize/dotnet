﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Stize.DotNet.Result;
using Stize.Persistence.Inquiry;

namespace Stize.Persistence.InquiryDispatcher
{
    public class InquiryDispatcher : IInquiryDispatcher
    {
        private readonly IInquiryHandlerFactory handlerFactory;

        public InquiryDispatcher(IInquiryHandlerFactory handlerFactory)
        {
            this.handlerFactory = handlerFactory;
        }

        public async Task<TResult> HandleAsync<TResult>(IInquiry<TResult> inquiry, CancellationToken cancellationToken = default)
            where TResult : class, IValueResult
        {

            var wrapper = (InquiryHandlerWrapper<TResult>)Activator.CreateInstance(typeof(InquiryHandlerWrapper<,>).MakeGenericType(inquiry.GetType(), typeof(TResult)));
            var result = await wrapper.HandleAsync(inquiry, handlerFactory, cancellationToken);
            return result;
        }


    }



}