using System;
using System.Collections.Generic;

namespace EasyNow.Utility.Collection
{
    public class TreeEntity<TKey,TEntity> where TKey:struct
    {
        /// <summary>
        /// Id
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public Nullable<TKey> ParentId { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public IEnumerable<TreeEntity<TKey, TEntity>> Children { get; set; }

        /// <summary>
        /// 实体
        /// </summary>
        public TEntity Entity { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }
    }
}