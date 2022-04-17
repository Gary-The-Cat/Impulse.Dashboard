using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvle.Domain
{
    public class GuessLetter
    {
        public Letter Letter;
        public Feedback Feedback;

        public GuessLetter(Letter letter, Feedback feedback)
        {
            Letter = letter;
            Feedback = feedback;
        }
    }
}
