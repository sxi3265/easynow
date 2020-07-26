using System;
using System.Collections.Generic;
using AutoMapper;

namespace EasyNow.Utility
{
    public static class UtilitySetup
    {
        private static IMapper _mapper;

        private static MapperConfiguration _mapperConfiguration;

        internal static IEnumerable<Type> profileTypes;

        public static IMapper Mapper
        {
            get
            {
                if (_mapper != null)
                {
                    return _mapper;
                }

                if (_mapperConfiguration != null)
                {
                    _mapper = _mapperConfiguration.CreateMapper();
                    return _mapper;
                }
                var config = new MapperConfiguration(c =>
                {
                    if (profileTypes != null)
                    {
                        foreach (var profileType in profileTypes)
                        {
                            c.AddProfile(profileType);
                        }
                    }
                    else
                    {
                        c.AddProfile<UtilityProfile>();
                    }
                });
                _mapper = config.CreateMapper();
                return _mapper;
            }
        }

        /// <summary>
        /// 初始化Utility设置
        /// </summary>
        /// <param name="mapperConfiguration"></param>
        public static void Init(MapperConfiguration mapperConfiguration)
        {
            _mapperConfiguration = mapperConfiguration;
        }

        /// <summary>
        /// 初始化Utility设置
        /// </summary>
        /// <param name="mapper"></param>
        public static void Init(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}