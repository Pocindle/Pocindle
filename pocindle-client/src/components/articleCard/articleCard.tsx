import React from 'react';
import './articleCard.scss';

interface article {
  url: string;
  title: string;
  excerpt?: string;
  listenDurationEstimate?: number;
  wordCount?: number;
}

export type { article };

const ArticleCard: React.FC<article> = ({
  url,
  title,
  excerpt,
  listenDurationEstimate,
  wordCount,
}) => {
  const handleUrlClick = (event: React.MouseEvent<HTMLSpanElement>) => {
    event.stopPropagation();
    window.open(url, '_blank');
  };

  return (
    <div className="article-card" onClick={() => console.log('123')}>
      <div className="article-card__wrapper">
        <div className="article-card__item">
          <span className="article-card__name">Title: </span>
          <span className="article-card__content article-card__title">
            {title}
          </span>
        </div>
        <div className="article-card__item">
          <span className="article-card__name">Article URL: </span>
          <span
            className="article-card__content article-card__link"
            onClick={handleUrlClick}
          >
            {url}
          </span>
        </div>
        <div className="article-card__item">
          <span className="article-card__name">Excerpt: </span>
          <span className="article-card__content">{excerpt}</span>
        </div>
        <div className="article-card__item">
          <span className="article-card__name">Listen duration: </span>
          <span className="article-card__content">
            {listenDurationEstimate + ' seconds'}
          </span>
        </div>
        <div className="article-card__item">
          <span className="article-card__name">Word count: </span>
          <span className="article-card__content">{wordCount + ' words'}</span>
        </div>
      </div>
    </div>
  );
};

export default ArticleCard;
