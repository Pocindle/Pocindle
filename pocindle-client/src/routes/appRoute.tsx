import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import {
  TestPage,
  AuthorizationPage,
  AuthorizationFinishedPage,
} from '../pages';
import PrivateRoute from './privateRoute';
import { postAuthRequest } from '../api/apiRequests';
import { useHistory } from 'react-router-dom';

const AppRouter: React.FC = () => {
  const history = useHistory();

  const handleAuthorization = () => {
    postAuthRequest();
    history.push('/authorizationFinished/test');
  };

  return (
    <Switch>
      <Route
        path="/auth"
        render={() => <AuthorizationPage onAuthorize={handleAuthorization} />}
      />
      <Route
        path="/authorizationFinished/:requestToken"
        component={AuthorizationFinishedPage}
      />
      <PrivateRoute exact path="/" component={TestPage} redirectPath="/auth" />
      <Redirect to="/" />
    </Switch>
  );
};

export default AppRouter;
