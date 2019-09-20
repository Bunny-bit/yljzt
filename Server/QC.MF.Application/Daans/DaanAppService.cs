using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QC.MF.Daans.Dto;
using QC.MF.Renyua;

namespace QC.MF.Daans
{
    public class DaanAppService : MFAppServiceBase, IDaanAppService
    {
        private readonly IRepository<Renyua1, int> _renyuanRepository;


        private readonly IRepository<Daan, int> _daanRepository;

        public DaanAppService(
            IRepository<Renyua1, int> renyuanRepository,
            IRepository<Daan, int> daanRepository
        )
        {
            _renyuanRepository = renyuanRepository;
            _daanRepository = daanRepository;
        }

        public async Task<AnswerOutput> Answer(AnswerInput input)
        {
            var renyuan = _renyuanRepository.Insert(new Renyua1
            {
                Name = input.Name,
                Xueyua = input.Xueyua,
                Xuehao = input.Xuehao,
                Banji = input.Banji,
                Code = ""
            });
            await CurrentUnitOfWork.SaveChangesAsync();

            foreach (var answer in input.Answers)
            {
                _daanRepository.Insert(new Daan
                {
                    RenyuanId = renyuan.Id,
                    TimuId = answer.TimuId,
                    XuanxiangId = answer.XuanxiangId
                });
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            return new AnswerOutput { Code = ""};
        }
    }
}
