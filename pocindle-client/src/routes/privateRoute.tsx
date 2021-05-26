import React from 'react';
import { Route, Redirect } from 'react-router-dom';

const PrivateRoute: React.FC<{
  component: React.FC<any>;
  isAuthorized?: boolean;
  redirectPath: string;
  exact?: boolean;
  path: string;
}> = ({
  component: Component,
  isAuthorized = false,
  redirectPath,
  exact,
  path,
}) => {
  return (
    <Route
      exact={exact ? true : false}
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
