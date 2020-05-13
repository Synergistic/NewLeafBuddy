import React, { useState, useEffect } from 'react';
import Login from '../Login';
import Logout from '../Logout';
import AddTown from './AddTown';
import TurnipPrices from './TurnipPrices';


const fruits = ["Apples", "Cherries", "Oranges", "Peaches", "Pears"];

const MyTown = props => {
    const [towns, setTowns] = useState([]);
    const [userName, setUserName] = useState(null);

    useEffect(() => {
        fetch(`https://acnlapi.azurewebsites.net/api/town/get?userName=${userName}`)
        .then(response => response.json())
        .then((data) => {
            setTowns(data);
        }
        );
    }, [userName]);


    const renderTowns = () => {
        if(userName && towns && towns.length > 0) {
            return (
                <React.Fragment>
                <ul>
                    <li>Town {towns[0].name}</li>
                    <li>Mayor {towns[0].mayorName}</li>
                    <li>Created {towns[0].created}</li>
                    <li>Fruit {fruits[towns[0].nativeFruit]}</li>
                </ul>
                <TurnipPrices town={towns[0]} />
                </React.Fragment>
            )
        }
        if(userName && (!towns || towns.length === 0)) {
            return <AddTown fruits={fruits} userName={userName} addNewTown={(towns) => setTowns(towns)}/>
        }
        return  <React.Fragment></React.Fragment>
    }
    
    const renderLogin = () => {
        if(userName){
            return <Logout performLogout={() => setUserName(null)} />
        }
        else {
            return <Login performLogin={(un) => setUserName(un)} />
        }
    }


    return (
        <div>
            {renderLogin()}
            {renderTowns()}            
        </div>
    );
}
export default MyTown;