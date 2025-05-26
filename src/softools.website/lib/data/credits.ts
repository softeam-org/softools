export type Contributor = {
    name: string;
    linkedin: string;
    image: string; 
};

export type ToolCredits = {
    tool: string;
    contributors: Contributor[];
};

export const credits: ToolCredits[] = [
    {
        tool: "Design",
        contributors: [
            {
                name: "Daniel Sena",
                linkedin: "https://www.linkedin.com/in/danielsena00/",
                image: "https://avatars.githubusercontent.com/u/45214080?v=4"
            },
        ]
    },
    {
        tool: "Geração de Documentos",
        contributors: [
            {
                name: "Thiago Menezes Vasconcelos",
                linkedin: "https://www.linkedin.com/in/thiago-m-vasconcelos",
                image: "https://avatars.githubusercontent.com/u/47324415?v=4"
            },
        ]
    },
    {
        tool: "Gerenciamento de Projetos",
        contributors: [
            {
                name: "Thiago Menezes Vasconcelos",
                linkedin: "https://www.linkedin.com/in/thiago-m-vasconcelos",
                image: "https://avatars.githubusercontent.com/u/47324415?v=4"
            },
            {
                name: "Edvaldo dos Santos",
                linkedin: "https://www.linkedin.com/in/edvaldo-dos-santos-7062a5217/",
                image: "https://media.licdn.com/dms/image/v2/D4D03AQG8AFrGYUQ_Yg/profile-displayphoto-shrink_200_200/B4DZR7BKQ8HIAY-/0/1737230690198?e=1753920000&v=beta&t=K3LsjasO_5NgtfwJbdrk-0kQIiZwdADSyTKwct9LPbE"
            },

        ]
    }
];