import React, { useState, useEffect, useRef } from 'react';
import { MainLayout } from '../../layouts';
import {
  ArticleCard,
  article,
  Loader,
  KindleEmailInput,
} from '../../components';
import {
  retrieveArticles,
  retrieveUserInfo,
  setKindleEmailAddress,
  sendArticleForConvertation,
  deliverArticle,
  retrieveServerEmailAddress,
} from '../../api/apiRequests';
import { getJwtTokenFromLocalStorage } from '../../utils/localStorage';
import './mainPage.scss';

const MainPage: React.FC<{ onLogOut: () => void }> = ({ onLogOut }) => {
  const [articles, setArticles] = useState<article[] | null>([]);
  const [articlesLoading, setArticlesLoading] = useState<boolean>(false);
  const [userInfoLoading, setUserInfoLoading] = useState<boolean>(false);
  const [serverEmailLoading, setServerEmailLoading] = useState<boolean>(false);
  const [searchInput, setSearchInput] = useState<string>('');
  const [kindleMail, setKindleMail] = useState<string>('');
  const username = useRef<string | null>(null);
  const jwtToken = useRef<string | null>(null);
  const serverEmail = useRef<string | null>(null);

  useEffect(() => {
    jwtToken.current = getJwtTokenFromLocalStorage();

    setArticlesLoading(true);
    setUserInfoLoading(true);
    setServerEmailLoading(true);

    retrieveUserInfo(jwtToken.current)
      .then((res) => {
        console.log(res.data);
        username.current = res.data.pocketUsername;
        setKindleMail(res.data.kindleEmailAddress || '');
        setUserInfoLoading(false);
      })
      .catch((err) => {
        console.log(err);
        setUserInfoLoading(false);
      });

    retrieveServerEmailAddress(jwtToken.current)
      .then((res) => {
        console.log(res.data);
        serverEmail.current = res.data.serverEmailAddress;
        setServerEmailLoading(false);
      })
      .catch((err) => {
        console.log(err);
        setServerEmailLoading(false);
      });

    retrieveArticles(jwtToken.current)
      .then((res) => {
        console.log(res.data);
        const articlesData = res.data.items.map((item) => {
          return {
            url: item.resolvedUrl,
            title: item.resolvedTitle,
            excerpt: item.excerpt,
            listenDurationEstimate: item.listenDurationEstimate,
            wordCount: item.wordCount,
          };
        });
        setArticles(articlesData);
        setArticlesLoading(false);
        console.log(articlesData);
      })
      .catch((err) => {
        setArticlesLoading(false);
        console.log(err);
      });
  }, []);

  const handleUpdateKindleEmail = (email: string) => {
    if (window.confirm('Set Kindle Email to ' + email + '?')) {
      setKindleEmailAddress(jwtToken.current || '', email)
        .then(() => {
          setKindleMail(email);
          alert('New Kindle email is set successfully!');
        })
        .catch((err) => {
          alert('An error occurred while setting new email!');
          console.log(err);
        });
    }
    console.log(email);
  };

  const handleArticleCardClick = (url: string) => {
    if (kindleMail) {
      if (window.confirm(`Send article (${url})?`)) {
        sendArticleForConvertation(jwtToken.current || '', url)
          .then((res) => {
            console.log(res.data);
            const deliveryId = res.data.deliveryId;
            deliverArticle(jwtToken.current || '', deliveryId)
              .then((res) => {
                console.log(res.data);
                alert('Successful delivery!');
              })
              .catch((err) => {
                console.log(err);
                alert('Delivery unsuccessful!');
              });
          })
          .catch((err) => {
            console.log(err);
            alert('Delivery unsuccessful!');
          });
      }
    } else {
      alert('Specify email!');
    }
  };

  const articleList = articles
    ? articles
        .filter((article) =>
          article.title.toLowerCase().includes(searchInput.toLowerCase())
        )
        .map((article) => (
          <ArticleCard
            key={article.url}
            url={article.url}
            title={article.title}
            excerpt={article.excerpt}
            wordCount={article.wordCount}
            onArticleCardClick={handleArticleCardClick}
          />
        ))
    : null;

  return (
    <MainLayout onLogOut={onLogOut}>
      <div className="main-page">
        <div className="main-page__wrapper">
          <div className="main-page__info-wrapper">
            <div className="main-page__info">
              <span className="main-page__info-title">{'Logged in as: '}</span>
              <span className="main-page__info-value">
                {userInfoLoading ? '...' : username.current}
              </span>
            </div>
            <div>
              <span className="main-page__info-title">{'Server email: '}</span>
              <span className="main-page__info-value">
                {serverEmailLoading ? '...' : serverEmail.current}
              </span>
            </div>
            <KindleEmailInput
              initialEmail={kindleMail}
              onUpdateEmail={handleUpdateKindleEmail}
            />
          </div>
          <span className="main-page__title">Your articles</span>
          <div className="main-page__search">
            <input
              type="text"
              placeholder="Search for article"
              value={searchInput}
              onChange={(event) => setSearchInput(event.target.value)}
              className="main-page__input"
            />
          </div>
          {articlesLoading ? (
            <div className="main-page__loader-wrapper">
              <Loader />
            </div>
          ) : articleList?.length ? (
            <div className="main-page__list">{articleList}</div>
          ) : (
            <div className="main-page__no-articles-message">NO ARTICLES :(</div>
          )}
        </div>
      </div>
    </MainLayout>
  );
};

export default MainPage;
