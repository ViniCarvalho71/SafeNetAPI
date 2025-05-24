using SafeNetAPI.Models;
using System.Reflection.Metadata.Ecma335;

namespace SafeNetAPI.Dto
{
    public class RequestListDto
    {

        public RequestListDto(List<RequestModel> requests, int quantidadeResgistros) {
            Requests = requests;
            QuantidadeRegistros = quantidadeResgistros;
        
        }
        public List<RequestModel> Requests { get; set; }

        public int QuantidadeRegistros { get; set; }

    }
}
