import React, { useState, useEffect } from 'react';
import { MainLayout } from '../../layouts';
import { ArticleCard, article, Loader } from '../../components';
import { retrieveArticles } from '../../api/apiRequests';
import { getJwtTokenFromLocalStorage } from '../../utils/localStorage';
import './mainPage.scss';

const MainPage: React.FC<{ onLogOut: () => void }> = ({ onLogOut }) => {
  const [articles, setArticles] = useState<article[] | null>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [searchInput, setSearchInput] = useState<string>('');

  useEffect(() => {
    const jwtToken = getJwtTokenFromLocalStorage();
    setLoading(true);
    retrieveArticles(jwtToken)
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
        setLoading(false);
        console.log(articlesData);
      })
      .catch((err) => {
        setLoading(false);
        console.log(err.message);
      });
  }, []);

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
            listenDurationEstimate={article.listenDurationEstimate}
            wordCount={article.wordCount}
          />
        ))
    : null;

  return (
    <MainLayout onLogOut={onLogOut}>
      <div className="main-page">
        <div className="main-page__wrapper">
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
          {loading ? (
            <div className="main-page__loader-wrapper">
              <Loader />
            </div>
          ) : articleList?.length ? (
            <div className="main-page__list">{articleList}</div>
          ) : (
            <div>NO ARTICLES :(</div>
          )}
        </div>
      </div>
    </MainLayout>
  );
};

export default MainPage;
