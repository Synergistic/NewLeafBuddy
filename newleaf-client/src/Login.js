import { GoogleLogin } from 'react-google-login';
import React, { useEffect } from 'react';

const Login = props => {
    const { performLogin } = props;

    useEffect(() => {
        let userName = localStorage.getItem('userName');
        if(userName) performLogin(userName);
    });

    const responseGoogle = (response) => {
        console.log(response);
    }

    const success = (response) => {
        let email = response.profileObj.email;
        let userName = email.replace('@', '');
        performLogin(userName)
        localStorage.clear();
        localStorage.setItem('userName', userName);
    }

    const getClientId = () => {
        if (window.location.href.indexOf("localhost") > -1) {
            return "296712442440-7ucpq9fvi7942h3pbf4ue59jtfl5119g.apps.googleusercontent.com";
        }
        return "296712442440-c4379tsukpnco35rr57d4as5uslqg0gl.apps.googleusercontent.com"
    }
    return (
        <GoogleLogin
            clientId={getClientId()}
            buttonText="Login"
            onSuccess={success}
            onFailure={responseGoogle}
            cookiePolicy={'single_host_origin'}
        />
    );
}
export default Login;