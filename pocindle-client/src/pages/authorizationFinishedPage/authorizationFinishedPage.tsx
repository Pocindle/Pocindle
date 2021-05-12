import React, { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import { AuthorizationLayout } from '../../layouts';
import { postRequestToken } from '../../api/apiRequests';
import './authorizationFinishedPage.scss';

const AuthorizationFinishedPage: React.FC<{
  onSuccessfulAuthorization: (jwtToken: string) => void;
}> = ({ onSuccessfulAuthorization }) => {
  const [isSuccessful, setIsSuccessful] = useState<boolean | null>(null);
  const { requestToken } = useParams<{ requestToken: string }>();

  useEffect(() => {
    const postRequest = () => {
      postRequestToken(requestToken, handleSuccessfulAuthorization, () =>
        setIsSuccessful(false)
      );
    };

    postRequest();
  }, []);

  function handleSuccessfulAuthorization(jwtToken: string) {
    onSuccessfulAuthorization(jwtToken);
    setIsSuccessful(true);
  }

  return (
    <AuthorizationLayout>
      <div className="authorization-finished-page">
        <div className="authorization-finished-page__wrapper">
          <div className="authorization-finished-page__message message">
            <div className="message__content">
              {typeof isSuccessful === 'object' ? (
                <span className="message__status">Waiting for result...</span>
              ) : isSuccessful === true ? (
                <React.Fragment>
                  <span className="message__status">
                    Authorization is successful!
                  </span>
                  <span className="message__redirect">
                    {'Go to '}
                    <Link to="/" className="message__link">
                      Pocindle
                    </Link>
                  </span>
                </React.Fragment>
              ) : (
                <React.Fragment>
                  <span className="message__status">Authorization error!</span>
                  <span className="message__redirect">
                    {'Return to '}
                    <Link to="/auth" className="message__link">
                      Authentification Page
                    </Link>
                  </span>
                </React.Fragment>
              )}
            </div>
          </div>
        </div>
      </div>
    </AuthorizationLayout>
  );
};

export default AuthorizationFinishedPage;
