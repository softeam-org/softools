import React from 'react';

interface IconSidebarProps {
  categories: string[];
  selected: string;
  onSelect: (category: string) => void;
}

const IconSidebar: React.FC<IconSidebarProps> = ({ categories, selected, onSelect }) => {
  return (
    <div className="w-24 bg-[var(--softeam1)] text-white flex flex-col items-center py-4 gap-4 shadow-xl shadow-black z-10">
      {categories.map((cat) => (
        <button
          key={cat}
          onClick={() => onSelect(cat)}
          className={`w-full text-sm text-center px-2 py-2 rounded hover:bg-[var(--softeam3)] ${
            selected === cat ? 'bg-[var(--softeam2)] font-semibold' : ''
          }`}
        >
          {cat}
        </button>
      ))}
    </div>
  );
};

export default IconSidebar;
