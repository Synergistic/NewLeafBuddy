import { Button } from 'semantic-ui-react'
import React from 'react'

const Logout = props => {
    const { performLogout } = props;

    const onClick = () => {
        performLogout();
        localStorage.clear();
    }

    return (
        <Button onClick={onClick}>
            Logout
        </Button>
    );
}
export default Logout;