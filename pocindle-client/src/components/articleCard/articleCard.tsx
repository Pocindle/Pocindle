import React from 'react';
import useAppContext from '../../hooks/useAppContext';
import './articleCard.scss';

interface article {
  url: string;
  title?: string;
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
  const { language } = useAppContext();

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
          <span className="article-card__name">
            {language.articleCard.title}
          </span>
          <span className="article-card__content article-card__title">
            {title ? title : language.common.notSpecified}
          </span>
        </div>
        <div className="article-card__item">
          <span className="article-card__name">
            {language.articleCard.articleUrl}
          </span>
          <span
            className="article-card__content article-card__link"
            onClick={handleUrlClick}
          >
            {url}
          </span>
        </div>
        <div className="article-card__item">
          <span className="article-card__name">
            {language.articleCard.excerpt}
          </span>
          <span className="article-card__content">
            {excerpt ? excerpt : language.common.notSpecified}
          </span>
        </div>
        <div className="article-card__item">
          <span className="article-card__name">
            {language.articleCard.wordCount}
          </span>
          <span className="article-card__content">
            {wordCount + language.articleCard.words}
          </span>
        </div>
      </div>
    </div>
  );
};

export default ArticleCard;
