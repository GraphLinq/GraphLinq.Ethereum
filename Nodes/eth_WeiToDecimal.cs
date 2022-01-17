package web3util

import (
	"math/big"
	"reflect"

	"github.com/blockc0de/engine/attributes"
	"github.com/blockc0de/engine/block"
	"github.com/shopspring/decimal"
)

func WeiToDecimal(value *big.Int, decimals int) decimal.Decimal {
	mul := decimal.NewFromFloat(float64(10)).Pow(decimal.NewFromFloat(float64(decimals)))
	num, _ := decimal.NewFromString(value.String())
	result := num.DivRound(mul, int32(decimals)).Truncate(int32(decimals))
	return result
}

var (
	fromWeiNodeDefinition       = []interface{}{attributes.NodeDefinition{NodeName: "FromWeiNode", FriendlyName: "Wei To Decimals", NodeType: attributes.NodeTypeEnumFunction, GroupName: "Web3.Util"}}
	fromWeiNodeGraphDescription = []interface{}{attributes.NodeGraphDescription{Description: "Converts any wei value into a decimals value"}}
)

type FromWeiNode struct {
	block.NodeBase
}

func NewFromWeiNode(id string, graph *block.Graph) (block.Node, error) {
	node := new(FromWeiNode)
	node.NodeData = block.NewNodeData(id, node, graph, reflect.TypeOf(node).String())

	wei, err := block.NewNodeParameter(node, "wei", block.NodeParameterTypeEnumDecimal, true, nil)
	if err != nil {
		return nil, err
	}
	node.NodeData.InParameters.Append(wei)

	decimals, err := block.NewNodeParameter(node, "decimals", block.NodeParameterTypeEnumDecimal, true, nil)
	if err != nil {
		return nil, err
	}
	node.NodeData.InParameters.Append(decimals)

	value, err := block.NewDynamicNodeParameter(node, "value", block.NodeParameterTypeEnumDecimal, false)
	if err != nil {
		return nil, err
	}
	node.NodeData.OutParameters.Append(value)

	return node, err
}

func (n *FromWeiNode) GetCustomAttributes(t reflect.Type) []interface{} {
	switch t {
	case reflect.TypeOf(attributes.NodeDefinition{}):
		return fromWeiNodeDefinition
	case reflect.TypeOf(attributes.NodeGraphDescription{}):
		return fromWeiNodeGraphDescription
	default:
		return nil
	}
}

func (n *FromWeiNode) ComputeParameterValue(parameterId string, value interface{}) interface{} {
	if parameterId == n.Data().OutParameters.Get("value").Id {
		var converter block.NodeParameterConverter
		wei, ok := converter.ToDecimal(n.Data().InParameters.Get("wei").ComputeValue())
		if !ok {
			return nil
		}

		decimals, ok := converter.ToDecimal(n.Data().InParameters.Get("decimals").ComputeValue())
		if !ok {
			decimals = decimal.NewFromInt(18)
		}

		return block.NodeParameterDecimal{Decimal: WeiToDecimal(wei.BigInt(), int(decimals.IntPart()))}
	}
	return value
}
