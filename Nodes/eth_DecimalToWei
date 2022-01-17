package web3util

import (
	"math/big"
	"reflect"

	"github.com/blockc0de/engine/attributes"
	"github.com/blockc0de/engine/block"
	"github.com/shopspring/decimal"
)

func DecimalToWei(amount decimal.Decimal, decimals int) *big.Int {
	mul := decimal.NewFromFloat(float64(10)).Pow(decimal.NewFromFloat(float64(decimals)))
	result := amount.Mul(mul)

	wei := big.NewInt(0)
	wei.SetString(result.String(), 10)
	return wei
}

var (
	toWeiNodeDefinition       = []interface{}{attributes.NodeDefinition{NodeName: "ToWeiNode", FriendlyName: "Decimals To Wei", NodeType: attributes.NodeTypeEnumFunction, GroupName: "Web3.Util"}}
	toWeiNodeGraphDescription = []interface{}{attributes.NodeGraphDescription{Description: "Converts any decimals value value into wei"}}
)

type ToWeiNode struct {
	block.NodeBase
}

func NewToWeiNode(id string, graph *block.Graph) (block.Node, error) {
	node := new(ToWeiNode)
	node.NodeData = block.NewNodeData(id, node, graph, reflect.TypeOf(node).String())

	value, err := block.NewNodeParameter(node, "value", block.NodeParameterTypeEnumDecimal, true, nil)
	if err != nil {
		return nil, err
	}
	node.NodeData.InParameters.Append(value)

	decimals, err := block.NewNodeParameter(node, "decimals", block.NodeParameterTypeEnumDecimal, true, nil)
	if err != nil {
		return nil, err
	}
	node.NodeData.InParameters.Append(decimals)

	wei, err := block.NewDynamicNodeParameter(node, "wei", block.NodeParameterTypeEnumDecimal, false)
	if err != nil {
		return nil, err
	}
	node.NodeData.OutParameters.Append(wei)

	return node, err
}

func (n *ToWeiNode) GetCustomAttributes(t reflect.Type) []interface{} {
	switch t {
	case reflect.TypeOf(attributes.NodeDefinition{}):
		return toWeiNodeDefinition
	case reflect.TypeOf(attributes.NodeGraphDescription{}):
		return toWeiNodeGraphDescription
	default:
		return nil
	}
}

func (n *ToWeiNode) ComputeParameterValue(parameterId string, value interface{}) interface{} {
	if parameterId == n.Data().OutParameters.Get("wei").Id {
		var converter block.NodeParameterConverter
		value, ok := converter.ToDecimal(n.Data().InParameters.Get("value").ComputeValue())
		if !ok {
			return nil
		}

		decimals, ok := converter.ToDecimal(n.Data().InParameters.Get("decimals").ComputeValue())
		if !ok {
			decimals = decimal.NewFromInt(18)
		}

		return block.NodeParameterDecimal{
			Decimal: decimal.NewFromBigInt(DecimalToWei(value, int(decimals.IntPart())), 0)}
	}
	return value
}
