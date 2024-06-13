using Examar.Core.Models;

namespace Examar.Core.Services;

public interface IElementDtoService
{
    Element ParseElement(ElementDto elementDto);
}
