using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvle.Domain;
public class LetterPosition
{
    public int ValidLetterInt;

    public LetterPosition (int position)
    {
        if (position <0 || position >4)
        {
            throw new ArgumentOutOfRangeException("position", "Position must be between 0 and 4");
        }

        ValidLetterInt = position;
    }
    
}
