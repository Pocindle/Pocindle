import React, { useState, useEffect, useRef } from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import {
  MainPage,
  AuthorizationPage,
  AuthorizationFinishedPage,
} from '../pages';
import PrivateRoute from './privateRoute';
import { postAuthRequest } from '../api/apiRequests';
import {
  getJwtTokenFromLocalStorage,
  setJwtTokenToLocalStorage,
  removeJwtTokenFromLocalStorage,
} from '../utils/localStorage';
import { setExpirationDateInterval } from '../utils/jwtTokenValidation';

const AppRouter: React.FC = () => {
  const [jwt, setJwt] = useState<string>(getJwtTokenFromLocalStorage() || '');
  const expirationDateInterval = useRef<number>(0);

  useEffect(() => {
    if (getJwtTokenFromLocalStorage()) {
      expirationDateInterval.current = setExpirationDateInterval(
        handleLogOut,
        30000
      );
      return () => {
        window.clearInterval(expirationDateInterval.current);
      };
    }
  }, [jwt]);

  const handleAuthorization = async () => {
    const { data } = await postAuthRequest();
    console.log(data);
    window.location.assign(data.redirectUrl);
  };

  const handleLogOut = () => {
    window.clearInterval(expirationDateInterval.current);
    removeJwtTokenFromLocalStorage();
    setJwt('');
  };

  const handleSuccessfulAuthorization = (jwtToken: string) => {
    setJwt(jwtToken);
    setJwtTokenToLocalStorage(jwtToken);
  };

  return (
    <Switch>
      <Route
        path="/auth"
        render={() => <AuthorizationPage onAuthorize={handleAuthorization} />}
      />
      <Route
        path="/authorizationFinished/:requestToken"
        render={() => (
          <AuthorizationFinishedPage
            onSuccessfulAuthorization={handleSuccessfulAuthorization}
          />
        )}
      />
      <PrivateRoute
        exact
        path="/"
        isAuthorized={!!jwt}
        component={() => <MainPage onLogOut={handleLogOut} />}
        redirectPath="/auth"
      />
      <Redirect to="/" />
    </Switch>
  );
};

export default AppRouter;
