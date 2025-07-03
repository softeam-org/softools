import React from 'react';

interface Category {
    name: string;
    icon?: React.ReactNode;
  }

  interface IconSidebarProps {
    categories: Category[];
    selected: string;
    onSelect: (category: string) => void;
  }

const IconSidebar: React.FC<IconSidebarProps> = ({ categories, selected, onSelect }) => {
    return (
      <div className="w-24 bg-[var(--softeam1)] text-white flex flex-col items-center py-4 gap-4 shadow-xl shadow-black z-10">
        {categories.map(({ name, icon }) => (
          <button
            key={name}
            onClick={() => onSelect(name)}
            className={`w-full flex justify-center text-sm px-2 py-2 rounded hover:bg-[var(--softeam3)] ${
              selected === name ? 'bg-[var(--softeam2)] font-semibold' : ''
            }`}
          >
            {icon ? icon : name}
          </button>
        ))}
      </div>
    );
  };
  

export default IconSidebar;
