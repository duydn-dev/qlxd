export class Constant {
    static readonly menuData: any = [
        {
            name: "Dashboard",
            link: "/",
            icon: "mdi me-2 mdi-home",
            selected: true,
            routerId: "dashboard",
            permission: []
        },
        {
            name: "Phòng ban",
            link: "/department",
            icon: "mdi me-2 mdi-home",
            selected: false,
            routerId: "department",
            permission: []
        },
        {
            name: "Hệ thống",
            icon: "mdi me-2 mdi-account-settings-variant",
            selected: false,
            routerId: null,
            permission: [],
            childs: [
                {
                    name: "Người dùng",
                    link: "/users",
                    selected: false,
                    routerId: 'users',
                    permission: [],
                },
                {
                    name: "Quyền hạn",
                    link: "/role",
                    selected: false,
                    routerId: 'role',
                    permission: [],
                },
            ]
        },
    ]
}