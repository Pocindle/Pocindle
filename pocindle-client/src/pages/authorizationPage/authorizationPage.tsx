import React from 'react';
import { AuthorizationLayout } from '../../layouts';
import useAppContext from '../../hooks/useAppContext';
import './authorizationPage.scss';

const AuthorizationPage: React.FC<{ onAuthorize: () => void }> = ({
  onAuthorize,
}) => {
  const handleAuthorization = () => {
    onAuthorize();
  };

  const { language } = useAppContext();

  return (
    <AuthorizationLayout>
      <div className="authorization-page">
        <div className="authorization-page__wrapper">
          <div className="authorization-page__auth auth">
            <h1 className="auth__title">{language.authorizationPage.title}</h1>
            <button
              className="auth__button"
              type="button"
              onClick={handleAuthorization}
            >
              {language.authorizationPage.login}
            </button>
          </div>
        </div>
      </div>
    </AuthorizationLayout>
  );
};

export default AuthorizationPage;
