import React from 'react';
import './articleCard.scss';

interface article {
  url: string;
  title: string;
  excerpt?: string;
  wordCount?: number;
  onArticleCardClick?: (url: string) => void;
}

export type { article };

const ArticleCard: React.FC<article> = ({
  url,
  title,
  excerpt,
  wordCount,
  onArticleCardClick,
}) => {
  const handleUrlClick = (event: React.MouseEvent<HTMLSpanElement>) => {
    event.stopPropagation();
    window.open(url, '_blank');
  };

  const handleArticleCardClick = () => {
    if (onArticleCardClick) onArticleCardClick(url);
  };

  return (
    <div className="article-card" onClick={handleArticleCardClick}>
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
          <span className="article-card__name">Word count: </span>
          <span className="article-card__content">{wordCount + ' words'}</span>
        </div>
      </div>
    </div>
  );
};

export default ArticleCard;
