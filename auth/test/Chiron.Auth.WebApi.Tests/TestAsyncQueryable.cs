using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Threading;

namespace Chiron.Auth.Tests
{
    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
	private readonly IQueryProvider _inner;

	internal TestAsyncQueryProvider(IQueryProvider inner)
	{
	    _inner = inner;
	}

	public IQueryable CreateQuery(Expression expression)
	{
	    return new TestAsyncEnumerable<TEntity>(expression);
	}

	public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
	{
	    return new TestAsyncEnumerable<TElement>(expression);
	}

	public object Execute(Expression expression)
	{
	    return _inner.Execute(expression);
	}

	public TResult Execute<TResult>(Expression expression)
	{
	    return _inner.Execute<TResult>(expression);
	}

	public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
	{
	    return new TestAsyncEnumerable<TResult>(expression);
	}

	public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
	{
	    return Task.FromResult(Execute<TResult>(expression));
	}
    }

    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
	public TestAsyncEnumerable(IEnumerable<T> enumerable)
	    : base(enumerable)
	{ }

	public TestAsyncEnumerable(Expression expression)
	    : base(expression)
	{ }

	public IAsyncEnumerator<T> GetEnumerator()
	{
	    return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
	}

	IQueryProvider IQueryable.Provider
	{
	    get { return new TestAsyncQueryProvider<T>(this); }
	}
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
	private readonly IEnumerator<T> _inner;

	public TestAsyncEnumerator(IEnumerator<T> inner)
	{
	    _inner = inner;
	}

	public void Dispose()
	{
	    _inner.Dispose();
	}

	public Task<bool> MoveNext(CancellationToken cancellationToken)
	{
	    return Task.FromResult(_inner.MoveNext());
	}

	public T Current
	{
	    get { return _inner.Current; }
	}
    }
}