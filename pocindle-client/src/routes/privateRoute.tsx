import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const PrivateRoute: React.FC<{
  component: React.FC<any>;
  isAuthorized?: boolean;
  redirectPath?: string;
  path: string;
}> = ({
  component: Component,
  isAuthorized = false,
  redirectPath = '/',
  path,
}) => {
  return (
    <Route
      path={path}
      render={(props) =>
        isAuthorized ? (
          <Component {...props} />
        ) : (
          <Redirect
            to={{ pathname: redirectPath, state: { from: props.location } }}
          />
        )
      }
    />
  );
};

export default PrivateRoute;
