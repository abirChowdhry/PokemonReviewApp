using System;

namespace PokemonReviewApp.Interfaces
{
    public interface ICrudRepo<TYPE, ID, RET, ID2>
    {
        List<TYPE> GetAll();
        TYPE Get(ID id);
        TYPE GetByName(ID2 name);
        RET Create(TYPE type);
        RET Update(TYPE type);
        RET Delete(ID id);
        RET Exst(ID id);
    }
}
