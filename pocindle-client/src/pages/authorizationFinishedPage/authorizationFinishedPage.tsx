import React, { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import { AuthorizationLayout } from '../../layouts';
import { postRequestToken } from '../../api/apiRequests';
import useAppContext from '../../hooks/useAppContext';
import './authorizationFinishedPage.scss';

const AuthorizationFinishedPage: React.FC<{
  onSuccessfulAuthorization: (jwtToken: string) => void;
}> = ({ onSuccessfulAuthorization }) => {
  const [isSuccessful, setIsSuccessful] = useState<boolean | null>(null);
  const { requestToken } = useParams<{ requestToken: string }>();

  const { language } = useAppContext();

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
                <span className="message__status">
                  {language.authorizationFinishedPage.statusWaiting}
                </span>
              ) : isSuccessful === true ? (
                <React.Fragment>
                  <span className="message__status">
                    {language.authorizationFinishedPage.successfulAuthorization}
                  </span>
                  <span className="message__redirect">
                    {language.authorizationFinishedPage.successfulRedirect}
                    <Link to="/" className="message__link">
                      {language.authorizationFinishedPage.successfulLink}
                    </Link>
                  </span>
                </React.Fragment>
              ) : (
                <React.Fragment>
                  <span className="message__status">
                    {language.authorizationFinishedPage.statusError}
                  </span>
                  <span className="message__redirect">
                    {language.authorizationFinishedPage.errorRedirect}
                    <Link to="/auth" className="message__link">
                      {language.authorizationFinishedPage.errorLink}
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
