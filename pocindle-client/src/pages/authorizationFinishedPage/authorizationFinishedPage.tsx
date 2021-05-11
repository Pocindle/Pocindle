import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { AuthorizationLayout } from '../../layouts';
import { postRequestToken } from '../../api/apiRequests';
import './authorizationFinishedPage.scss';

const AuthorizationFinishedPage: React.FC = () => {
  const [isSuccessful, setIsSuccessful] = useState<boolean | null>(null);
  const { requestToken } = useParams<{ requestToken: string }>();

  useEffect(() => {
    const postRequest = () => {
      postRequestToken(
        requestToken,
        () => setIsSuccessful(true),
        () => setIsSuccessful(false)
      );
    };

    postRequest();
  }, []);

  return (
    <AuthorizationLayout>
      <div className="authorization-finished-page">
        <div className="authorization-finished-page__wrapper">
          <div className="authorization-finished-page__message message">
            <div className="message__content">
              {typeof isSuccessful === null ? (
                <div>Waiting for result</div>
              ) : isSuccessful ? (
                <div>success</div>
              ) : (
                <div>error</div>
              )}
              <span>{`Message: ${requestToken}`}</span>
            </div>
          </div>
        </div>
      </div>
    </AuthorizationLayout>
  );
};

export default AuthorizationFinishedPage;
