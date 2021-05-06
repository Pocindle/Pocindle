import React from 'react';
import { AuthorizationLayout } from '../../layouts';
import './authorizationPage.scss';

const AuthorizationPage: React.FC<{ onAuthorize: () => null }> = () => {
  return (
    <AuthorizationLayout>
      <div className="authorization-page">
        <div className="authorization-page__wrapper">
          <div className="authorization-page__auth auth">
            <h1 className="auth__title">Authorize through Pocket</h1>
            <button className="auth__button">Login</button>
          </div>
        </div>
      </div>
    </AuthorizationLayout>
  );
};

export default AuthorizationPage;
