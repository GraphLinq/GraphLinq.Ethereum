﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Nethereum;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.Contracts
{
    class StandardNftContract : ContractDeploymentMessage
    {
        public static string BYTECODE = "0x60806040523480156200001157600080fd5b50604051620036103803806200361083398181016040528101906200003791906200032c565b60016000806301ffc9a760e01b7bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916815260200190815260200160002060006101000a81548160ff02191690831515021790555060016000806380ac58cd60e01b7bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916815260200190815260200160002060006101000a81548160ff0219169083151502179055506001600080635b5e139f60e01b7bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916815260200190815260200160002060006101000a81548160ff02191690831515021790555033600860006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550816005908051906020019062000183929190620001e7565b5080600690805190602001906200019c929190620001e7565b5082600860006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555050505062000598565b828054620001f5906200048f565b90600052602060002090601f01602090048101928262000219576000855562000265565b82601f106200023457805160ff191683800117855562000265565b8280016001018555821562000265579182015b828111156200026457825182559160200191906001019062000247565b5b50905062000274919062000278565b5090565b5b808211156200029357600081600090555060010162000279565b5090565b6000620002ae620002a884620003ef565b620003c6565b905082815260208101848484011115620002cd57620002cc6200055e565b5b620002da84828562000459565b509392505050565b600081519050620002f3816200057e565b92915050565b600082601f83011262000311576200031062000559565b5b81516200032384826020860162000297565b91505092915050565b60008060006060848603121562000348576200034762000568565b5b60006200035886828701620002e2565b935050602084015167ffffffffffffffff8111156200037c576200037b62000563565b5b6200038a86828701620002f9565b925050604084015167ffffffffffffffff811115620003ae57620003ad62000563565b5b620003bc86828701620002f9565b9150509250925092565b6000620003d2620003e5565b9050620003e08282620004c5565b919050565b6000604051905090565b600067ffffffffffffffff8211156200040d576200040c6200052a565b5b62000418826200056d565b9050602081019050919050565b6000620004328262000439565b9050919050565b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b60005b83811015620004795780820151818401526020810190506200045c565b8381111562000489576000848401525b50505050565b60006002820490506001821680620004a857607f821691505b60208210811415620004bf57620004be620004fb565b5b50919050565b620004d0826200056d565b810181811067ffffffffffffffff82111715620004f257620004f16200052a565b5b80604052505050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052602260045260246000fd5b7f4e487b7100000000000000000000000000000000000000000000000000000000600052604160045260246000fd5b600080fd5b600080fd5b600080fd5b600080fd5b6000601f19601f8301169050919050565b620005898162000425565b81146200059557600080fd5b50565b61306880620005a86000396000f3fe608060405234801561001057600080fd5b506004361061012c5760003560e01c806395d89b41116100ad578063d0def52111610071578063d0def52114610343578063d31af4841461035f578063e985e9c51461037b578063f2fde38b146103ab578063f3fe3bc3146103c75761012c565b806395d89b411461029f578063a22cb465146102bd578063b88d4fde146102d9578063c87b56dd146102f5578063d082e381146103255761012c565b806342842e0e116100f457806342842e0e146101e75780636352211e1461020357806370a0823114610233578063860d248a146102635780638da5cb5b146102815761012c565b806301ffc9a71461013157806306fdde0314610161578063081812fc1461017f578063095ea7b3146101af57806323b872dd146101cb575b600080fd5b61014b60048036038101906101469190612a5f565b6103e5565b6040516101589190612c48565b60405180910390f35b61016961044c565b6040516101769190612c63565b60405180910390f35b61019960048036038101906101949190612ab9565b6104de565b6040516101a69190612be1565b60405180910390f35b6101c960048036038101906101c49190612a1f565b6105f9565b005b6101e560048036038101906101e091906128a4565b6109dc565b005b61020160048036038101906101fc91906128a4565b610e2e565b005b61021d60048036038101906102189190612ab9565b610e4e565b60405161022a9190612be1565b60405180910390f35b61024d60048036038101906102489190612837565b610f34565b60405161025a9190612c85565b60405180910390f35b61026b610fee565b6040516102789190612c63565b60405180910390f35b610289611027565b6040516102969190612be1565b60405180910390f35b6102a761104d565b6040516102b49190612c63565b60405180910390f35b6102d760048036038101906102d2919061297f565b6110df565b005b6102f360048036038101906102ee91906128f7565b6111dc565b005b61030f600480360381019061030a9190612ab9565b611233565b60405161031c9190612c63565b60405180910390f35b61032d6113b6565b60405161033a9190612c85565b60405180910390f35b61035d600480360381019061035891906129bf565b6113bc565b005b61037960048036038101906103749190612ae6565b6114fe565b005b61039560048036038101906103909190612864565b6115d5565b6040516103a29190612c48565b60405180910390f35b6103c560048036038101906103c09190612837565b611669565b005b6103cf61189b565b6040516103dc9190612c63565b60405180910390f35b6000806000837bffffffffffffffffffffffffffffffffffffffffffffffffffffffff19167bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916815260200190815260200160002060009054906101000a900460ff169050919050565b60606005805461045b90612e6e565b80601f016020809104026020016040519081016040528092919081815260200182805461048790612e6e565b80156104d45780601f106104a9576101008083540402835291602001916104d4565b820191906000526020600020905b8154815290600101906020018083116104b757829003601f168201915b5050505050905090565b600081600073ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f3030333030320000000000000000000000000000000000000000000000000000815250906105bc576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016105b39190612c63565b60405180910390fd5b506002600084815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16915050919050565b8060006001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1690503373ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1614806106f25750600460008273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060009054906101000a900460ff165b6040518060400160405280600681526020017f303033303033000000000000000000000000000000000000000000000000000081525090610769576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016107609190612c63565b60405180910390fd5b5082600073ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303032000000000000000000000000000000000000000000000000000081525090610846576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161083d9190612c63565b60405180910390fd5b5060006001600086815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1690508073ffffffffffffffffffffffffffffffffffffffff168673ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303038000000000000000000000000000000000000000000000000000081525090610926576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161091d9190612c63565b60405180910390fd5b50856002600087815260200190815260200160002060006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550848673ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b92560405160405180910390a4505050505050565b8060006001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1690503373ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff161480610aad57503373ffffffffffffffffffffffffffffffffffffffff166002600084815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16145b80610b3e5750600460008273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060009054906101000a900460ff165b6040518060400160405280600681526020017f303033303034000000000000000000000000000000000000000000000000000081525090610bb5576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610bac9190612c63565b60405180910390fd5b5082600073ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303032000000000000000000000000000000000000000000000000000081525090610c92576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610c899190612c63565b60405180910390fd5b5060006001600086815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1690508673ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff16146040518060400160405280600681526020017f303033303037000000000000000000000000000000000000000000000000000081525090610d71576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610d689190612c63565b60405180910390fd5b50600073ffffffffffffffffffffffffffffffffffffffff168673ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303031000000000000000000000000000000000000000000000000000081525090610e1a576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610e119190612c63565b60405180910390fd5b50610e2586866118d4565b50505050505050565b610e4983838360405180602001604052806000815250611989565b505050565b60006001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff169050600073ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303032000000000000000000000000000000000000000000000000000081525090610f2e576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610f259190612c63565b60405180910390fd5b50919050565b60008073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303031000000000000000000000000000000000000000000000000000081525090610fdd576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610fd49190612c63565b60405180910390fd5b50610fe782611f57565b9050919050565b6040518060400160405280600681526020017f303138303032000000000000000000000000000000000000000000000000000081525081565b600860009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1681565b60606006805461105c90612e6e565b80601f016020809104026020016040519081016040528092919081815260200182805461108890612e6e565b80156110d55780601f106110aa576101008083540402835291602001916110d5565b820191906000526020600020905b8154815290600101906020018083116110b857829003601f168201915b5050505050905090565b80600460003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060006101000a81548160ff0219169083151502179055508173ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff167f17307eab39ab6107e8899845ad3d59bd9653f200f220920489ca2b5937696c31836040516111d09190612c48565b60405180910390a35050565b61122c85858585858080601f016020809104026020016040519081016040528093929190818152602001838380828437600081840152601f19601f82011690508083019250505050505050611989565b5050505050565b606081600073ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303032000000000000000000000000000000000000000000000000000081525090611311576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016113089190612c63565b60405180910390fd5b5060076000848152602001908152602001600020805461133090612e6e565b80601f016020809104026020016040519081016040528092919081815260200182805461135c90612e6e565b80156113a95780601f1061137e576101008083540402835291602001916113a9565b820191906000526020600020905b81548152906001019060200180831161138c57829003601f168201915b5050505050915050919050565b60095481565b600860009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16146040518060400160405280600681526020017f303138303031000000000000000000000000000000000000000000000000000081525090611484576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161147b9190612c63565b60405180910390fd5b5060006009600081548092919061149a90612ed1565b9190505590506114aa8482611fa0565b6114f88184848080601f016020809104026020016040519081016040528093929190818152602001838380828437600081840152601f19601f8201169050808301925050505050505061218e565b50505050565b600860009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16146040518060400160405280600681526020017f3031383030310000000000000000000000000000000000000000000000000000815250906115c6576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016115bd9190612c63565b60405180910390fd5b506115d1828261218e565b5050565b6000600460008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060009054906101000a900460ff16905092915050565b600860009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff16146040518060400160405280600681526020017f303138303031000000000000000000000000000000000000000000000000000081525090611731576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016117289190612c63565b60405180910390fd5b50600073ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f3031383030320000000000000000000000000000000000000000000000000000815250906117da576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016117d19190612c63565b60405180910390fd5b508073ffffffffffffffffffffffffffffffffffffffff16600860009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a380600860006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555050565b6040518060400160405280600681526020017f303138303031000000000000000000000000000000000000000000000000000081525081565b60006001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905061191582612298565b61191f81836122d1565b611929838361243c565b818373ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef60405160405180910390a4505050565b8160006001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1690503373ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff161480611a5a57503373ffffffffffffffffffffffffffffffffffffffff166002600084815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16145b80611aeb5750600460008273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060003373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060009054906101000a900460ff165b6040518060400160405280600681526020017f303033303034000000000000000000000000000000000000000000000000000081525090611b62576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401611b599190612c63565b60405180910390fd5b5083600073ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303032000000000000000000000000000000000000000000000000000081525090611c3f576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401611c369190612c63565b60405180910390fd5b5060006001600087815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1690508773ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff16146040518060400160405280600681526020017f303033303037000000000000000000000000000000000000000000000000000081525090611d1e576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401611d159190612c63565b60405180910390fd5b50600073ffffffffffffffffffffffffffffffffffffffff168773ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303031000000000000000000000000000000000000000000000000000081525090611dc7576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401611dbe9190612c63565b60405180910390fd5b50611dd287876118d4565b611df18773ffffffffffffffffffffffffffffffffffffffff166125c4565b15611f4d5760008773ffffffffffffffffffffffffffffffffffffffff1663150b7a02338b8a8a6040518563ffffffff1660e01b8152600401611e379493929190612bfc565b602060405180830381600087803b158015611e5157600080fd5b505af1158015611e65573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190611e899190612a8c565b905063150b7a0260e01b7bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916817bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916146040518060400160405280600681526020017f303033303035000000000000000000000000000000000000000000000000000081525090611f4a576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401611f419190612c63565b60405180910390fd5b50505b5050505050505050565b6000600360008373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020549050919050565b600073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f303033303031000000000000000000000000000000000000000000000000000081525090612048576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161203f9190612c63565b60405180910390fd5b50600073ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16146040518060400160405280600681526020017f303033303036000000000000000000000000000000000000000000000000000081525090612123576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161211a9190612c63565b60405180910390fd5b5061212e828261243c565b808273ffffffffffffffffffffffffffffffffffffffff16600073ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef60405160405180910390a45050565b81600073ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1614156040518060400160405280600681526020017f30303330303200000000000000000000000000000000000000000000000000008152509061226a576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016122619190612c63565b60405180910390fd5b508160076000858152602001908152602001600020908051906020019061229292919061260f565b50505050565b6002600082815260200190815260200160002060006101000a81549073ffffffffffffffffffffffffffffffffffffffff021916905550565b8173ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16146040518060400160405280600681526020017f3030333030370000000000000000000000000000000000000000000000000000815250906123aa576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016123a19190612c63565b60405180910390fd5b506001600360008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282546123fb9190612d84565b925050819055506001600082815260200190815260200160002060006101000a81549073ffffffffffffffffffffffffffffffffffffffff02191690555050565b600073ffffffffffffffffffffffffffffffffffffffff166001600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16146040518060400160405280600681526020017f303033303036000000000000000000000000000000000000000000000000000081525090612516576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161250d9190612c63565b60405180910390fd5b50816001600083815260200190815260200160002060006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055506001600360008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282546125b99190612d2e565b925050819055505050565b60008060007fc5d2460186f7233c927e7db2dcc703c0e500b653ca82273b7bfad8045d85a47060001b9050833f91506000801b82141580156126065750808214155b92505050919050565b82805461261b90612e6e565b90600052602060002090601f01602090048101928261263d5760008555612684565b82601f1061265657805160ff1916838001178555612684565b82800160010185558215612684579182015b82811115612683578251825591602001919060010190612668565b5b5090506126919190612695565b5090565b5b808211156126ae576000816000905550600101612696565b5090565b60006126c56126c084612cc5565b612ca0565b9050828152602081018484840111156126e1576126e0612fb6565b5b6126ec848285612e2c565b509392505050565b60008135905061270381612fd6565b92915050565b60008135905061271881612fed565b92915050565b60008135905061272d81613004565b92915050565b60008151905061274281613004565b92915050565b60008083601f84011261275e5761275d612fac565b5b8235905067ffffffffffffffff81111561277b5761277a612fa7565b5b60208301915083600182028301111561279757612796612fb1565b5b9250929050565b60008083601f8401126127b4576127b3612fac565b5b8235905067ffffffffffffffff8111156127d1576127d0612fa7565b5b6020830191508360018202830111156127ed576127ec612fb1565b5b9250929050565b600082601f83011261280957612808612fac565b5b81356128198482602086016126b2565b91505092915050565b6000813590506128318161301b565b92915050565b60006020828403121561284d5761284c612fc0565b5b600061285b848285016126f4565b91505092915050565b6000806040838503121561287b5761287a612fc0565b5b6000612889858286016126f4565b925050602061289a858286016126f4565b9150509250929050565b6000806000606084860312156128bd576128bc612fc0565b5b60006128cb868287016126f4565b93505060206128dc868287016126f4565b92505060406128ed86828701612822565b9150509250925092565b60008060008060006080868803121561291357612912612fc0565b5b6000612921888289016126f4565b9550506020612932888289016126f4565b945050604061294388828901612822565b935050606086013567ffffffffffffffff81111561296457612963612fbb565b5b61297088828901612748565b92509250509295509295909350565b6000806040838503121561299657612995612fc0565b5b60006129a4858286016126f4565b92505060206129b585828601612709565b9150509250929050565b6000806000604084860312156129d8576129d7612fc0565b5b60006129e6868287016126f4565b935050602084013567ffffffffffffffff811115612a0757612a06612fbb565b5b612a138682870161279e565b92509250509250925092565b60008060408385031215612a3657612a35612fc0565b5b6000612a44858286016126f4565b9250506020612a5585828601612822565b9150509250929050565b600060208284031215612a7557612a74612fc0565b5b6000612a838482850161271e565b91505092915050565b600060208284031215612aa257612aa1612fc0565b5b6000612ab084828501612733565b91505092915050565b600060208284031215612acf57612ace612fc0565b5b6000612add84828501612822565b91505092915050565b60008060408385031215612afd57612afc612fc0565b5b6000612b0b85828601612822565b925050602083013567ffffffffffffffff811115612b2c57612b2b612fbb565b5b612b38858286016127f4565b9150509250929050565b612b4b81612db8565b82525050565b612b5a81612dca565b82525050565b6000612b6b82612cf6565b612b758185612d0c565b9350612b85818560208601612e3b565b612b8e81612fc5565b840191505092915050565b6000612ba482612d01565b612bae8185612d1d565b9350612bbe818560208601612e3b565b612bc781612fc5565b840191505092915050565b612bdb81612e22565b82525050565b6000602082019050612bf66000830184612b42565b92915050565b6000608082019050612c116000830187612b42565b612c1e6020830186612b42565b612c2b6040830185612bd2565b8181036060830152612c3d8184612b60565b905095945050505050565b6000602082019050612c5d6000830184612b51565b92915050565b60006020820190508181036000830152612c7d8184612b99565b905092915050565b6000602082019050612c9a6000830184612bd2565b92915050565b6000612caa612cbb565b9050612cb68282612ea0565b919050565b6000604051905090565b600067ffffffffffffffff821115612ce057612cdf612f78565b5b612ce982612fc5565b9050602081019050919050565b600081519050919050565b600081519050919050565b600082825260208201905092915050565b600082825260208201905092915050565b6000612d3982612e22565b9150612d4483612e22565b9250827fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff03821115612d7957612d78612f1a565b5b828201905092915050565b6000612d8f82612e22565b9150612d9a83612e22565b925082821015612dad57612dac612f1a565b5b828203905092915050565b6000612dc382612e02565b9050919050565b60008115159050919050565b60007fffffffff0000000000000000000000000000000000000000000000000000000082169050919050565b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b6000819050919050565b82818337600083830152505050565b60005b83811015612e59578082015181840152602081019050612e3e565b83811115612e68576000848401525b50505050565b60006002820490506001821680612e8657607f821691505b60208210811415612e9a57612e99612f49565b5b50919050565b612ea982612fc5565b810181811067ffffffffffffffff82111715612ec857612ec7612f78565b5b80604052505050565b6000612edc82612e22565b91507fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff821415612f0f57612f0e612f1a565b5b600182019050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601160045260246000fd5b7f4e487b7100000000000000000000000000000000000000000000000000000000600052602260045260246000fd5b7f4e487b7100000000000000000000000000000000000000000000000000000000600052604160045260246000fd5b600080fd5b600080fd5b600080fd5b600080fd5b600080fd5b600080fd5b6000601f19601f8301169050919050565b612fdf81612db8565b8114612fea57600080fd5b50565b612ff681612dca565b811461300157600080fd5b50565b61300d81612dd6565b811461301857600080fd5b50565b61302481612e22565b811461302f57600080fd5b5056fea2646970667358221220211396e73625e6b209c76f84c336461808f5244fc608f12f68eabf0853b1945564736f6c63430008060033";

        public StandardNftContract() : base(BYTECODE) { }

        [Parameter("address", "_owner")]
        public string Owner { get; set; }


        [Parameter("string", "_nftName")]
        public string Name { get; set; }


        [Parameter("string", "_nftSymbol")]
        public string Symbol { get; set; }

        [Function("mint", "uint256")]
        public class MintFunction : FunctionMessage
        {
            [Parameter("address", "_to", 1)]
            public string MintTo { get; set; }

            [Parameter("string", "_uri", 2)]
            public string TokenUri { get; set; }
        }
    }
}
