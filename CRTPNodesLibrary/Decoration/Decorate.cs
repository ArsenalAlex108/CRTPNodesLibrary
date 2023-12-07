using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FakeItEasy;

namespace CRTPNodesLibrary.Decoration;
public static class Decorate
{
    public static T ToString<T>(T obj, Func<T, string> newFunc) where T : class
    {
        var result = A.Fake<T>(i => i.Wrapping(obj));

        _ = A.CallTo(() => result.ToString()).ReturnsLazily(() => newFunc(obj));

        return result;
    }

    public static T ToString<T>(T obj, Func<string> newFunc) where T : class
    {
        var result = A.Fake<T>(i => i.Wrapping(obj));

        _ = A.CallTo(() => result.ToString()).ReturnsLazily(newFunc);

        return result;
    }
}
