using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueTeamTriviaMaze
{
    public interface TriviaItemFactory
    {
        TriviaItem GenerateTriviaItem();
        void Destroy();
    }
}
