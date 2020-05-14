import React, { useState, useEffect } from 'react';
import { Button, Label, Table, Input } from 'semantic-ui-react'
import _ from 'lodash';

const days = ['Sunday(Buy)', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

const TurnipPrices = props => {
    const { town } = props;
    const [morningPrices, setMorningPrices] = useState([]);
    const [afternoonPrices, setAfternoonPrices] = useState([]);
    const [enteredTurnips, setEnteredTurnips] = useState(0);
    const [editingPrice, setEditingPrice] = useState(-1);
    const [editingType, setEditingType] = useState(-1);

    useEffect(() => {
        if (town) {
            let allPrices = town.turnipPrices.split('.');
            filterPrices(allPrices);
        }
    }, [town]);

    const filterPrices = (prices) => {
        setMorningPrices(prices.filter((p, i) => {
            return i % 2 === 0;
        }));
        setAfternoonPrices(prices.filter((p, i) => {
            return i % 2 === 1;
        }));
    }

    const updateTurnipPrices = () => {
        setEditingType(-1);
        setEditingPrice(-1);
        debugger;
        let allPrices = [];
        morningPrices.forEach((mp, i) => {
            allPrices.push(mp);
            allPrices.push(afternoonPrices[i]);
        })
        let turnipPricesString = allPrices.join(".");

        fetch(`https://acnlapi.azurewebsites.net/api/town/UpdateTurnips?userName=${town.ownerUsername}&townName=${town.name}&turnipPrices=${turnipPricesString}&quantity=${enteredTurnips}`)
            .then(response => response.json())
            .then((data) => {
                debugger;
                let allPrices = data.turnipPrices.split('.');
                filterPrices(allPrices);
            }
            );
    }

    const clickEditableCell = (cellType, index) => {
        setEditingType(cellType)
        setEditingPrice(index);
    }

    const updatePrice = (pricesToUse, newValue, index, type) => {
        var copy = pricesToUse.slice();
        copy[index] = newValue;
        if (type === 0) {
            setMorningPrices(copy);
        }
        else {
            setAfternoonPrices(copy);
        }
    }

    const renderCells = (pricesToUse, type) => {
        return pricesToUse.map((p, i) => {
            if (editingPrice === i && type === editingType) {
                return (

                    <Table.Cell selectable textAlign='center'>
                        <Input
                            onChange={(e) => updatePrice(pricesToUse, e.target.value, i, type)}
                        />
                    </Table.Cell>
                )
            }
            else {

                return (
                    <Table.Cell selectable textAlign='center' onClick={() => clickEditableCell(type, i)}>{p}</Table.Cell>
                );
            }
        })
    }
    const renderProfits = () => {
        let buyPrice = morningPrices[0];
        let morningProfits = morningPrices.map((p, i) => {
            if (i === 0) return i;
            return (town.turnipsOwned * p) - (town.turnipsOwned * buyPrice);
        })
        let afternoonProfits = afternoonPrices.map((p, i) => {
            if (i === 0) return i;
            return (town.turnipsOwned * p) - (town.turnipsOwned * buyPrice);
        })
        let maximumProfits = [0, 0, 0, 0, 0, 0, 0].map((p, i) => {
            return morningProfits[i] > afternoonProfits[i] ? morningProfits[i] : afternoonProfits[i];
        })

        return maximumProfits.map((p, i) => {
            if(i !== 0){
                return (
                    <Table.Cell textAlign='center' >{p}</Table.Cell>
                );
            }
        })
    }

    return (
        <React.Fragment>
            <Table definition unstackable>
                <Table.Header>
                    <Table.Row>
                        <Table.Cell><Label ribbon color='red'>Turnips</Label></Table.Cell>
                        {_.map(days, (d) => (
                            <Table.HeaderCell>{d}</Table.HeaderCell>
                        ))}
                    </Table.Row>
                </Table.Header>

                <Table.Body>
                    <Table.Row>
                        <Table.Cell>Morning</Table.Cell>
                        {renderCells(morningPrices, 0)}
                    </Table.Row>
                    <Table.Row>
                        <Table.Cell>Afternoon</Table.Cell>
                        {renderCells(afternoonPrices, 1)}
                    </Table.Row>
                    <Table.Row>
                        <Table.Cell>Potential Profits</Table.Cell>
                        <Table.Cell textAlign='center'>N/A</Table.Cell>
                        {renderProfits()}
                    </Table.Row>
                </Table.Body>
            </Table>
            <Button
                primary
                size='large'
                onClick={updateTurnipPrices}
            >
                Save
          </Button>
            <Input
                label={{ tag: true, content: 'Add Turnips', color: 'red' }}
                labelPosition='right'
                placeholder='Quantity...'
                defaultValue={enteredTurnips}
                onChange={(e) =>  setEnteredTurnips(e.target.value)}
            />
        </React.Fragment>
    );
}
export default TurnipPrices;