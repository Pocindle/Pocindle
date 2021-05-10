import React from 'react';
import { useParams } from 'react-router-dom';
import { AuthorizationLayout } from '../../layouts';
import './authorizationFinishedPage.scss';

const AuthorizationFinishedPage: React.FC = () => {
  const { requestToken } = useParams<{ requestToken: string }>();

  return (
    <AuthorizationLayout>
      <div className="authorization-finished-page">
        <div className="authorization-finished-page__wrapper">
          <div className="authorization-finished-page__message message">
            <div className="message__content">
              <span>{`Message: ${requestToken}`}</span>
            </div>
          </div>
        </div>
      </div>
    </AuthorizationLayout>
  );
};

export default AuthorizationFinishedPage;
