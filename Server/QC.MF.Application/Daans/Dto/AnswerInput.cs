using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QC.MF.Daans.Dto
{

    public class AnswerDto
    {
        public int TimuId { get; set; }

        public int XuanxiangId { get; set; }
    }

    public class AnswerInput
    {

        public string Name { get; set; }


        public string Xueyua { get; set; }


        public string Xuehao { get; set; }


        public string Banji { get; set; }


        public List<AnswerDto> Answers { get; set; }

    }
}
